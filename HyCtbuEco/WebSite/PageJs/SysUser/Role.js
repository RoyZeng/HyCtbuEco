
function SysRoleRunPage() {


   options = {

        renderDiv: $("#SysRoleTree"),
        hidId: $("#hidId"),
        frmElement: $("#frmSysRole"),



        UpdateBtn: $("#btnSysRoleUpdate"),
        AddBtn: $("#btnSysRoleAdd"),
        DelBtn: $("#btnSysRoleDel"),


        //节点数据
        NodeValue: ko.observable({

            Id: 0, SRoleName: '', SDis: '', SMem: '', SPower: '', ILevel: 0, IParentId: 0

        }),

        //用户查询URL
        dataQueryUrlAccessor: function () { return "/WebApi/SysRole/TreeAll"; },

        //添加用户URL
        dataAddUrlAccessor: function (key) { return "/WebApi/SysRole/AddTreeNode/" + key; },
        dataUpdateAccessor: function (key) { return "/WebApi/SysRole/Put/" + key; },
        dataDeleteAccessor: function (key) { return "/WebApi/SysRole/DelTreeNode/" + key; },
        dataNodeSelect: function (id) { return "/WebApi/SysRole/get/" + id; }

        , showDlgBefore: function () {
           

        }
            , showDetailEnd: function (data) {


            }
         , changeEnd: function (key, event) {
             //对话框保存的后置事件
         }, initEd: function () {
             //gird初始化的后置事件 信息

         }

   };

    viewModel = new ViewModel(options);

    viewModel.init();
    ko.applyBindings(viewModel,c1);
    viewModel.showPower = function () {
        //显示对话框之前
        //alert("显示对话框");
        //此时需要根据信息来checked
        var tmpStr = viewModel.NodeValue().SPower;
        //首先，清除所有的check元素；
        viewModel.zTreeObj.checkAllNodes(false);
        if (tmpStr!=null && tmpStr != "") {
            var tmpStrs = tmpStr.split(',');
            for (var i = 0; i < tmpStrs.length; i++) {
                if (tmpStrs[i] != "") {
                    // ,viewModel.zTreeObj.getNodeByTId(tmpStrs[i]);,,,.zTreeObj.getNodeByParam("id", tmpStrs[i], null); 
                    var selNode = viewModel.zTreeObj.getNodeByParam("id", tmpStrs[i], null); //如果有高度节点，则选中它
                    viewModel.zTreeObj.checkNode(selNode, true, true);//勾选节点
                }
            }

        };

    }


    options.frmElement.validate({
        rules: {


            txtbId: { required: true, digits: true }, txtbSRoleName: { maxlength: 50 }, txtbSDis: { maxlength: 50 }, txtbSMem: { maxlength: 50 }, txtbILevel: { digits: true }, txtbIParentID: { digits: true }



        },
        messages: {


            txtbId: { required: '必填项', digits: '必须是整数' }, txtbSRoleName: { maxlength: '最大长度为200' }, txtbSDis: { maxlength: '最大长度为16' }, txtbSMem: { maxlength: '最大长度为500' }, txtbILevel: { digits: '必须是整数' }, txtbIParentId: { digits: '必须是整数' }

        }
    });

    //获取PowerTree数据并显示
    $.ajax({
        type: "post",
        url: "/WebApi/SysPagePower/TreeAll"
    }).done(function (data) {
        var setting = {
            view: {
                showIcon: true
            },
            check: {
                enable: true,
                chkboxType: { "Y": "s", "N": "" }  //勾选时，父关联子
            },
            data: {
                simpleData: {
                    enable: true
                }
            }
        };

        //打开Power树
       
        var treeNodes = data;
        viewModel.zTreeObj=$.fn.zTree.init($("#Powertree"), setting, treeNodes);
    });

    //组装选中的权限id
    $("#confirm_power").click(function () {
        var PowerIds = "";
        var zTree = $.fn.zTree.getZTreeObj("Powertree");
        var check = zTree.getCheckedNodes(true);//取得所有被选中节点数据
        for (var i in check) {
            PowerIds = PowerIds + "," + check[i].id;
        }
        PowerIds = dropRsplit(PowerIds, ",");//去除多余的','分割符
        options.NodeValue().SPower = PowerIds;//将组装后的权限id绑定到NodeValue的SPower上
        $("#txtbSPower").val(PowerIds);//将组装后的权限id显示到SPower的input框中            


        //隐藏对话框并取消遮罩
        $("#confirm_power").attr("data-dismiss", "modal");
        $("#confirm_power").attr("aria-hidden", "true");
    });
}

