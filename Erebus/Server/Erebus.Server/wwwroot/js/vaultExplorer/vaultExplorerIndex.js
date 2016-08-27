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

    vaultExplorerIndex.onTreeNodeChange = function (e, data) {
        if (data.action === "select_node") {
            if (data.node) {
                $("#" + addSubGroupButtonId).prop("disabled", false);
                $("#" + editGroupButtonId).prop("disabled", false);
                $("#" + deleteGroupButtonId).prop("disabled", false);
                $("#" + addEntryButtonId).prop("disabled", false);

                vaultExplorerIndex.loadGroupEntries(data.node.id);
            }
            else {
                $("#" + addSubGroupButtonId).prop("disabled", true);
                $("#" + editGroupButtonId).prop("disabled", true);
                $("#" + deleteGroupButtonId).prop("disabled", true);
                $("#" + addEntryButtonId).prop("disabled", true);
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

    vaultExplorerIndex.refreshNode = function (nodeId) {
        var tree = $("#" + treeId).jstree();
        var node = tree.get_node(nodeId);
        var parentNode = tree.get_node(node.parent);
        tree.load_node(node);
        tree.open_node(node);
        tree.load_node(parentNode);
        tree.open_node(parentNode);
    };

    //vaultExplorerIndex.openNode = function (nodeId) {
    //    var node = $("#" + treeId).jstree().get_node(nodeId);
    //    $("#" + treeId).jstree().open_node(node);
    //};

    //vaultExplorerIndex.refreshTree = function () {
    //    $("#" + treeId).jstree().refresh()
    //};

    vaultExplorerIndex.submitEditForm = function (formId, modalId, groupId, refreshEntries) {
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
                            vaultExplorerIndex.refreshNode(node.parent);
                            if (refreshEntries) {
                                vaultExplorerIndex.loadGroupEntries(groupId);
                            }
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
