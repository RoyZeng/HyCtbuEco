//RunPageParts
function SysPartRunPage() {
    options = {

        renderDiv: $("#SyspartTree"),
        hidId: $("#hidId"),
        frmElement: $("#frmSyspart"),



        UpdateBtn: $("#btnSyspartUpdate"),
        AddBtn: $("#btnSyspartAdd"),
        DelBtn: $("#btnSyspartDel"),


        //节点数据
        NodeValue: ko.observable({

            Id: 0, SPartName: '', IType: 0,SKey:'',
            SScript: '', IParentId: 0, SMemo: '', IOwer: 0, IIsTmp: 0, ILevel: 0
            ,SLatitude:'',SLong:''

        }),

        //用户查询URL
        dataQueryUrlAccessor: function () { return "/WebApi/Syspart/TreeAll"; },

        //添加用户URL
        dataAddUrlAccessor: function (key) { return "/WebApi/Syspart/AddTreeNode/" + key; },
        dataUpdateAccessor: function (key) { return "/WebApi/Syspart/Put/" + key; },
        dataDeleteAccessor: function (key) {


            return "/WebApi/Syspart/DelTreeNode/" + key;

        },
        dataNodeSelect: function (id) {
            return "/WebApi/Syspart/get/" + id;
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

    //地图区域查找并显示在地图上
    viewModel.MapSearch = function () {
        

    }

    viewModel.init();
    ko.applyBindings(viewModel,c1);


    //$("#myform").validate({ , debug: true })

    options.frmElement.validate({
        rules: {

            txtbSPartName: { required: true, maxlength: 50 },
            txtbSKey: { required: true, maxlength:20 },
            txtbIType: { required: true, digits: true },
            txtbSScript: { maxlength: 50 },
            txtbSMemo: { maxlength: 50 },
            txtbIIsTmp: { digits: true },
            txtbSLong: { number: true },
            txtbSLatitude: { number: true }

        },
        messages: {

            txtbSPartName: { required: '必填项', maxlength: '最大长度为50' },
            txtbSKey: { required: '必填项', maxlength: '最大长度为20' },
            txtbIType: { required: '必填项', digits: '必须是整数' },
            txtbSScript: { maxlength: '最大长度为50' },
            txtbSMemo: { maxlength: '最大长度为50' },
            txtbIIsTmp: { digits: '必须是整数' },
            txtbSLong: { number: '必须是数字' },
            txtbSLatitude: { number: '必须是数字' }

        },
        highlight: function (element) {
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
        },
        errorElement: 'span',
        errorClass: 'help-block'

    });

}//end of RunPage();
