var vaultExplorerIndex = (function (vaultExplorerIndex) {

    var treeId = "vault-tree";
    var entriesGridId = "entries-grid";
    var addGroupButtonId = "add-group-button";
    var addSubGroupButtonId = "add-sub-group-button";
    var editGroupButtonId = "edit-group-button";
    var deleteGroupButtonId = "delete-group-button";
    var addEntryButtonId = "add-entry-button";

    vaultExplorerIndex.init = function () {
        //Tree creation
        $("#" + treeId).jstree({
            'core': {
                'data': {
                    'url': function (node) {
                        return "/VaultExplorer/GetNodeItems";
                    },
                    'data': function (node) {
                        return { 'parentId': node.id };
                    }
                }
            }
        });

        //Event binding
        $("#" + addGroupButtonId).on("click", vaultExplorerIndex.addGroup);
        $("#" + addSubGroupButtonId).on("click", vaultExplorerIndex.addSubGroup);
        $("#" + editGroupButtonId).on("click", vaultExplorerIndex.editGroup);
        $("#" + deleteGroupButtonId).on("click", vaultExplorerIndex.deleteGroup);
        $("#" + addEntryButtonId).on("click", vaultExplorerIndex.addEntry);
        $('#' + treeId).on('changed.jstree', vaultExplorerIndex.onTreeNodeChange);
        $(window).on("resize", function () {
            $(".vault-explorer-entries-table-wrapper").height(
                $(window).height() - $(".btn-toolbar").height()
             );
        });
    };

    vaultExplorerIndex.enableButtons = function () {
        $("#" + addSubGroupButtonId).prop("disabled", false);
        $("#" + editGroupButtonId).prop("disabled", false);
        $("#" + deleteGroupButtonId).prop("disabled", false);
        $("#" + addEntryButtonId).prop("disabled", false);
    };

    vaultExplorerIndex.disableButtons = function () {
        $("#" + addSubGroupButtonId).prop("disabled", true);
        $("#" + editGroupButtonId).prop("disabled", true);
        $("#" + deleteGroupButtonId).prop("disabled", true);
        $("#" + addEntryButtonId).prop("disabled", true);
    };

    vaultExplorerIndex.onTreeNodeChange = function (e, data) {
        if (data.action === "select_node") {
            if (data.node) {
                vaultExplorerIndex.enableButtons();
                vaultExplorerIndex.loadGroupEntries(data.node.id);
            }
            else {
                vaultExplorerIndex.disableButtons();
            }
        }
    };

    vaultExplorerIndex.loadGroupEntries = function (groupId) {
        $.ajax({
            url: "/VaultExplorer/GroupEntriesGrid",
            method: "GET",
            data: {
                groupId: groupId
            }
        }).done(function (html) {
            $("#" + entriesGridId).html(html);
            sessionTimeout.reset();
        });
    };

    vaultExplorerIndex.addGroup = function () {
        $.ajax({
            url: "/VaultExplorer/AddOrEditGroup",
            method: "GET",
            data: {
                parentId: "#"
            }
        }).done(function (html) {
            $("body").append(html);
            sessionTimeout.reset();
        });
    };

    vaultExplorerIndex.addSubGroup = function () {
        var selectedNodes = $("#" + treeId).jstree().get_selected(true);
        if (selectedNodes.length === 1) {
            var node = selectedNodes[0];

            $.ajax({
                url: "/VaultExplorer/AddOrEditGroup",
                method: "GET",
                data: {
                    parentId: node.id
                }
            }).done(function (html) {
                $("body").append(html);
                sessionTimeout.reset();
            });
        }
    };

    vaultExplorerIndex.editGroup = function () {
        var selectedNodes = $("#" + treeId).jstree().get_selected(true);
        if (selectedNodes.length === 1) {
            var node = selectedNodes[0];

            $.ajax({
                url: "/VaultExplorer/AddOrEditGroup",
                method: "GET",
                data: {
                    id: node.id,
                    parentId: node.parent
                }
            }).done(function (html) {
                $("body").append(html);
                sessionTimeout.reset();
            });
        }
    };

    vaultExplorerIndex.deleteGroup = function () {
        var selectedNodes = $("#" + treeId).jstree().get_selected(true);
        if (selectedNodes.length === 1) {
            var node = selectedNodes[0];

            if (confirm("Group and all it's children will be deleted. Are you sure?")) {

                $.ajax({
                    url: "/VaultExplorer/DeleteGroup",
                    method: "POST",
                    data: {
                        id: node.id
                    }
                })
                .done(function () {
                    vaultExplorerIndex.refreshNode(node.parent);
                    sessionTimeout.reset();
                });
            }
        }
    };


    vaultExplorerIndex.addEntry = function () {
        var selectedNodes = $("#" + treeId).jstree().get_selected(true);
        if (selectedNodes.length === 1) {
            var node = selectedNodes[0];

            $.ajax({
                url: "/VaultExplorer/AddOrEditEntry",
                method: "GET",
                data: {
                    parentId: node.id
                }
            }).done(function (html) {
                $("body").append(html);
                sessionTimeout.reset();
            });
        }
    };


    vaultExplorerIndex.editEntry = function (entryId) {
        var selectedNodes = $("#" + treeId).jstree().get_selected(true);
        if (selectedNodes.length === 1) {
            var node = selectedNodes[0];

            $.ajax({
                url: "/VaultExplorer/AddOrEditEntry",
                method: "GET",
                data: {
                    id: entryId,
                    parentId: node.id
                }
            }).done(function (html) {
                $("body").append(html);
                sessionTimeout.reset();
            });
        }
    };

    vaultExplorerIndex.deleteEntry = function (entryId) {

        if (confirm("Entry will be deleted. Are you sure?")) {

            $.ajax({
                url: "/VaultExplorer/DeleteEntry",
                method: "POST",
                data: {
                    id: entryId
                }
            })
            .done(function () {
                var selectedNodes = $("#" + treeId).jstree().get_selected(true);
                if (selectedNodes.length === 1) {
                    var node = selectedNodes[0];
                    vaultExplorerIndex.refreshNode(node.parent, node.id);
                    vaultExplorerIndex.loadGroupEntries(node.id);
                }
                sessionTimeout.reset();
            });
        }
    };


    vaultExplorerIndex.refreshNode = function (nodeId, afterRefreshSelectId) {
        var tree = $("#" + treeId).jstree();
        var node = tree.get_node(nodeId);
        tree.load_node(node, function () {
            if (afterRefreshSelectId) {
                tree.select_node(afterRefreshSelectId, true);
                vaultExplorerIndex.enableButtons();
            } else {
                if (!tree.get_selected().length) {
                    vaultExplorerIndex.disableButtons();
                }
            }

        });
        tree.open_node(node);
    };

    vaultExplorerIndex.submitGroupEditForm = function (formId, modalId, parentNodeId) {
        $("#" + formId).one("submit",function (e) {
            if ($(this).valid()) {
                var postData = $(this).serializeArray();
                var formURL = $(this).attr("action");

                $.ajax({
                    url: formURL,
                    type: "POST",
                    data: postData,
                    success: function (data, textStatus, jqXHR) {
                        if (data.success) {
                            vaultExplorerIndex.closeModal(modalId);
                            vaultExplorerIndex.refreshNode(parentNodeId);
                        }
                        sessionTimeout.reset();
                    }
                });
            }
            e.preventDefault();
        });

        $("#" + formId).submit();
    };

    vaultExplorerIndex.submitEntryEditForm = function (formId, modalId, groupId) {
        $("#" + formId).one("submit",function (e) {
            if ($(this).valid()) {
                var postData = $(this).serializeArray();
                var formURL = $(this).attr("action");

                $.ajax({
                    url: formURL,
                    type: "POST",
                    data: postData,
                    success: function (data, textStatus, jqXHR) {
                        if (data.success) {
                            vaultExplorerIndex.closeModal(modalId);
                            var tree = $("#" + treeId).jstree();
                            var node = tree.get_node(groupId);
                            vaultExplorerIndex.refreshNode(node.parent, node.id);
                            vaultExplorerIndex.loadGroupEntries(groupId);
                        }
                        sessionTimeout.reset();
                    }
                });
            }
            e.preventDefault();
        });

        $("#" + formId).submit();
    };

    vaultExplorerIndex.closeModal = function (modalId) {
        $("#" + modalId).on("hidden.bs.modal", function () {
            $("#" + modalId).remove();
        });
        $("#" + modalId).modal('hide');
    };


    vaultExplorerIndex.toggleShowPassword = function (buttonElement, inputId) {
        if ($("#" + inputId).prop("type") === "password") {
            $("#" + inputId).prop("type", "text");

            $(buttonElement).addClass("btn-warning");
            $(buttonElement).removeClass("btn-primary");
            $(buttonElement).find("span").addClass("glyphicon-eye-close");
            $(buttonElement).find("span").removeClass("glyphicon-eye-open");

        } else {
            $("#" + inputId).prop("type", "password");

            $(buttonElement).addClass("btn-primary");
            $(buttonElement).removeClass("btn-warning");
            $(buttonElement).find("span").addClass("glyphicon-eye-open");
            $(buttonElement).find("span").removeClass("glyphicon-eye-close");
        }

    };


    function copyToClipboard(elem) {
        // create hidden text element, if it doesn't already exist
        var targetId = "_hiddenCopyText_";
        var isInput = elem.tagName === "INPUT" || elem.tagName === "TEXTAREA";
        var origSelectionStart, origSelectionEnd;
        if (isInput) {
            // can just use the original source element for the selection and copy


            target = elem;
            origSelectionStart = elem.selectionStart;
            origSelectionEnd = elem.selectionEnd;
        } else {
            // must use a temporary form element for the selection and copy
            target = document.getElementById(targetId);
            if (!target) {
                var target = document.createElement("textarea");
                target.style.position = "absolute";
                target.style.left = "-9999px";
                target.style.top = "0";
                target.id = targetId;
                document.body.appendChild(target);
            }
            target.textContent = elem.textContent;
        }
        // select the content
        var currentFocus = document.activeElement;
        target.focus();
        target.setSelectionRange(0, target.value.length);

        // copy the selection
        var succeed;
        try {
            succeed = document.execCommand("copy");
        } catch (e) {
            succeed = false;
        }
        // restore original focus
        if (currentFocus && typeof currentFocus.focus === "function") {
            currentFocus.focus();
        }

        if (isInput) {
            // restore prior selection
            elem.setSelectionRange(origSelectionStart, origSelectionEnd);
        } else {
            // clear temporary content
            target.textContent = "";
        }
        return succeed;
    }

    vaultExplorerIndex.copyPassword = function (inputId) {
        if ($("#" + inputId).prop("type") === "password") {
            $("#" + inputId).prop("type", "text");
            copyToClipboard($("#" + inputId)[0]);
            $("#" + inputId).prop("type", "password");
        } else {
            copyToClipboard($("#" + inputId)[0]);
        }

        $.notify("Copied!", { autoHide: true, autoHideDelay: 1500, className: "info", position: "top center" });
    };

    vaultExplorerIndex.generatePassword = function (entryId) {
        $.ajax({
            url: "/VaultExplorer/GeneratePassword",
            method: "GET",
            data: {
                id: entryId
            }
        }).done(function (html) {
            $("body").append(html);
            sessionTimeout.reset();
        });
    };

    vaultExplorerIndex.submitGeneratePasswordForm = function (formId, inputId) {
        $("#" + formId).one("submit",function (e) {
            if ($(this).valid()) {
                var postData = $(this).serializeArray();
                var formURL = $(this).attr("action");

                $.ajax({
                    url: formURL,
                    type: "POST",
                    data: postData,
                    success: function (data, textStatus, jqXHR) {
                        if (data.success) {
                            $("#" + inputId).val(data.password);
                        }
                        sessionTimeout.reset();
                    }
                });
            }
            e.preventDefault();
        });

        $("#" + formId).submit();
    };

   

    vaultExplorerIndex.acceptGeneratedPassword = function (modalId, targetInputId, generatedPasswordInputId) {
        $("#" + targetInputId).val($("#" + generatedPasswordInputId).val());
        vaultExplorerIndex.closeModal(modalId);
    };

    return vaultExplorerIndex;
}(vaultExplorerIndex || {}));


$(function () {
    vaultExplorerIndex.init();
})
