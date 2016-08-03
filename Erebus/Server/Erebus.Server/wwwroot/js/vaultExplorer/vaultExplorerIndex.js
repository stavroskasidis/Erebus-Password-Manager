var vaultExplorerIndex = (function (vaultExplorerIndex) {

    var treeId = "vault-tree";
    var addGroupButtonId = "add-group-button";
    var addSubGroupButtonId = "add-sub-group-button";
    var editGroupButtonId = "edit-group-button";
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
        $("#" + addEntryButtonId).on("click", vaultExplorerIndex.addEntry);
        $('#' + treeId).on('changed.jstree', vaultExplorerIndex.onTreeNodeChange);
    };

    vaultExplorerIndex.onTreeNodeChange = function (e, data) {
        if (data.node && data.node.li_attr && data.node.li_attr["node-type"] === "group") {
            $("#" + addSubGroupButtonId).prop("disabled", false);
            $("#" + editGroupButtonId).prop("disabled", false);
            $("#" + addEntryButtonId).prop("disabled", false);
        }
        else {
            $("#" + addSubGroupButtonId).prop("disabled", true);
            $("#" + editGroupButtonId).prop("disabled", true);
            $("#" + addEntryButtonId).prop("disabled", true);
        }
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


    vaultExplorerIndex.addEntry = function () {
    };

    vaultExplorerIndex.refreshNode = function (nodeId) {
        var tree = $("#" + treeId).jstree();
        var node = tree.get_node(nodeId);
        tree.load_node(node);
        tree.open_node(node);
    };

    //vaultExplorerIndex.openNode = function (nodeId) {
    //    var node = $("#" + treeId).jstree().get_node(nodeId);
    //    $("#" + treeId).jstree().open_node(node);
    //};

    //vaultExplorerIndex.refreshTree = function () {
    //    $("#" + treeId).jstree().refresh()
    //};

    vaultExplorerIndex.submitModalForm = function (formId, modalId, parentNodeId) {
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
