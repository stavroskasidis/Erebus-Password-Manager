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

    //vaultExplorerIndex.openNode = function (nodeId) {
    //    var node = $("#" + treeId).jstree().get_node(nodeId);
    //    $("#" + treeId).jstree().open_node(node);
    //};

    //vaultExplorerIndex.refreshTree = function () {
    //    $("#" + treeId).jstree().refresh()
    //};

    vaultExplorerIndex.submitGroupEditForm = function (formId, modalId, parentNodeId) {
        $("#" + formId).submit(function (e) {
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
                    }
                });
            }
            e.preventDefault();
        });

        $("#" + formId).submit();
    };

    vaultExplorerIndex.submitEntryEditForm = function (formId, modalId, groupId) {
        $("#" + formId).submit(function (e) {
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

    return vaultExplorerIndex;
}(vaultExplorerIndex || {}));


$(function () {
    vaultExplorerIndex.init();
})
