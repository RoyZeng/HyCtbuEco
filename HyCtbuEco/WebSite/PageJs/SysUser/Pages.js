//RunPagePages
function SysPageRunPage() {
    

     options = {

        renderDiv: $("#SysPagePowerTree"),
        hidId: $("#hidId"),
        frmElement: $("#frmSysPagePower"),



        UpdateBtn: $("#btnSysPagePowerUpdate"),
        AddBtn: $("#btnSysPagePowerAdd"),
        DelBtn: $("#btnSysPagePowerDel"),


        //节点数据
        NodeValue: ko.observable({

            Id: 0, SMenuName: '', SMenuURL: '', IParentID: 0, ILevel: 0, IPowerID: 0, IType: 0, SPicUrl: '', IMenuType: 0, ISort: 0, IIsDelete: 0

        }),

        //用户查询URL
        dataQueryUrlAccessor: function () { return "/WebApi/SysPagePower/TreeAll"; },

        //添加用户URL
        dataAddUrlAccessor: function (key) { return "/WebApi/SysPagePower/AddTreeNode/" + key; },
        dataUpdateAccessor: function (key) { return "/WebApi/SysPagePower/Put/" + key; },
        dataDeleteAccessor: function (key) {


            return "/WebApi/SysPagePower/DelTreeNode/" + key;

        },
        dataNodeSelect: function (id) {
            return "/WebApi/SysPagePower/Get/" + id;
        }

        , showDlgBefore: function () {
            //显示对话框之前

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




    options.frmElement.validate({
        rules: {


             txtbSMenuName: { maxlength: 50 }, txtbSMenuURL: { maxlength: 50 },    txtbIType: { digits: true }, txtbSPicUrl: { maxlength: 50 }, txtbIMenuType: { digits: true }



        },
        messages: {


             txtbSMenuName: { maxlength: '最大长度为250' }, txtbSMenuURL: { maxlength: '最大长度为2000' },   txtbIType: { digits: '必须是整数' }, txtbSPicUrl: { maxlength: '最大长度为250' }, txtbIMenuType: { digits: '必须是整数' }

        }, highlight: function (element) {
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
        },
        errorElement: 'span',
        errorClass: 'help-block'
    });

}