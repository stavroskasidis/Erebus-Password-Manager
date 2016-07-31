var homeIndex = (function (homeIndex) {
    
    var treeId = "vault-tree";
    var addGroupButtonId = "add-group-button";
    var addEntryButtonId = "add-entry-button";

    homeIndex.onTreeNodeChange = function (e, data) {
        if (data.node.li_attr["node-type"] === "group") {
            $("#" + addGroupButtonId).prop("disabled", false);
            $("#" + addEntryButtonId).prop("disabled", false);
        }
        else {
            $("#" + addGroupButtonId).prop("disabled", true);
            $("#" + addEntryButtonId).prop("disabled", true);
        }
    };

    homeIndex.addGroup = function () {
    }

    homeIndex.addEntry = function () {
    }

    homeIndex.init = function () {

        //Tree creation
        $("#" + treeId).jstree({
            'core': {
                'data': {
                    'url': function (node) {
                        return "/Home/GetNodeItems";
                    },
                    'data': function (node) {
                        return { 'parentId': node.id };
                    }
                }
            }
        });

        //Event binding
        $("#" + addGroupButtonId).on("click", homeIndex.addGroup);
        $("#" + addEntryButtonId).on("click", homeIndex.addEntry);
        $('#' + treeId).on('changed.jstree', homeIndex.onTreeNodeChange);
    };

    homeIndex.refreshTree = function () {
        $("#" + treeId).jstree().refresh()
    }

    return homeIndex;
}(homeIndex || {}));


$(function () {
    homeIndex.init();
})
