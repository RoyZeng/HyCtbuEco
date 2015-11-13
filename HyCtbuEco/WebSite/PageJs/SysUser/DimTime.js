//开始DimTime.js 脚本
function DimTimeRunPage() {


    options = {

        renderDiv: $("#DimTimeTree"),
        hidId: $("#hidId"),
        frmElement: $("#frmDimTime"),



        UpdateBtn: $("#btnDimTimeUpdate"),
        AddBtn: $("#btnDimTimeAdd"),
        DelBtn: $("#btnDimTimeDel"),


        //节点数据
        NodeValue: ko.observable({

            Id: 0,
            SName: '',
            STimeCode: '',
            IParentID: 0,
            ILevel: 0,
            SMemo: '',
            ILibID:0

        }),

        //用户查询URL
        dataQueryUrlAccessor: function () { return "/WebApi/MyDimTime/GetBySelLibId?id=" + $("#ILibID").val(); },

        //添加用户URL
        dataAddUrlAccessor: function (key) {
            options.NodeValue().ILibID = $("#ILibID").val();
            return "/WebApi/DimTime/AddTreeNode/" + key;
        },
        dataUpdateAccessor: function (key) { return "/WebApi/DimTime/Put/" + key; },
        dataDeleteAccessor: function (key) {


            return "/WebApi/DimTime/DelTreeNode/" + key;

        },
        dataNodeSelect: function (id) {
            return "/WebApi/DimTime/get/" + id;
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
            txtbSName: { maxlength: 50 },
            txtbSTimeCode: { maxlength: 50 },
            txtbIParentID: { digits: true },
            txtbILevel: { digits: true },
            txtbSMemo: { maxlength: 50 },




        },
        messages: {


            txtbId: { required: '必填项', digits: '必须是整数' },
            txtbSName: { maxlength: '最大长度为10' },
            txtbSTimeCode: { maxlength: '最大长度为50' },
            txtbIParentID: { digits: '必须是整数' },
            txtbILevel: { digits: '必须是整数' },
            txtbSMemo: { maxlength: '最大长度为50' },


        }
    });


    $("#ILibID").change(function () {
        viewModel.init();
    });
};