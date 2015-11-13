//开始DimArea.js 脚本
function DimAreaRunPage() {


    options = {

        renderDiv: $("#DimAreaTree"),
        hidId: $("#hidId"),
        frmElement: $("#frmDimArea"),

        UpdateBtn: $("#btnDimAreaUpdate"),
        AddBtn: $("#btnDimAreaAdd"),
        DelBtn: $("#btnDimAreaDel"),


        //节点数据
        NodeValue: ko.observable({

            Id: 0,
            SAreaCode: '',
            SAreaName: '',
            IParentID: 0,
            SMemo: '',
            ILevel: 0,
            SPost: '',
            ILibID:0

        }),

        //用户查询URL
        dataQueryUrlAccessor: function () { return "/WebApi/MyDimArea/GetBySelLibId?id=" + $("#ILibID").val(); },

        //添加用户URL
        dataAddUrlAccessor: function (key) {
            options.NodeValue().ILibID = $("#ILibID").val();
            return "/WebApi/DimArea/AddTreeNode/" + key;
        },
        dataUpdateAccessor: function (key) { return "/WebApi/DimArea/Put/" + key; },
        dataDeleteAccessor: function (key) {


            return "/WebApi/DimArea/DelTreeNode/" + key;

        },
        dataNodeSelect: function (id) {
            return "/WebApi/DimArea/get/" + id;
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
    ko.applyBindings(viewModel, c1);




    options.frmElement.validate({
        rules: {


            txtbId: { required: true, digits: true },
            txtbSAreaCode: { maxlength: 50 },
            txtbSAreaName: { maxlength: 50 },
            txtbIParentID: { digits: true },
            txtbSMemo: { maxlength: 50 },
            txtbILevel: { digits: true },
            txtbSPost: { maxlength: 50 },




        },
        messages: {


            txtbId: { required: '必填项', digits: '必须是整数' },
            txtbSAreaCode: { maxlength: '最大长度为10' },
            txtbSAreaName: { maxlength: '最大长度为50' },
            txtbIParentID: { digits: '必须是整数' },
            txtbSMemo: { maxlength: '最大长度为50' },
            txtbILevel: { digits: '必须是整数' },
            txtbSPost: { maxlength: '最大长度为10' },


        }
    });

    $("#ILibID").change(function () {
        viewModel.init();
    });

};