var setting = {
    view: {
        showIcon: true
    },
    check: {
        enable: true
    },
    data: {
        simpleData: {
            enable: true
        }
    }
};

//得到角色数据，并初始化角色数
function getRoleData() {
    $.ajax({
        url: '/api/RoleManage',
        type: 'get',
        dataType: 'json',
        async: true,
        success: function (data, text) {
            $.fn.zTree.init($("#roleTree"), setting, data);
        },
        error: function () {
            alert("获取角色数据出错！");
        }
    });
}








$(document).ready(function () {
    $.fn.zTree.init($("#roleTree"));
    getRoleData();
});