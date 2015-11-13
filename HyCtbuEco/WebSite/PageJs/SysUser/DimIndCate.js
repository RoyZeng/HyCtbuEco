//开始DimIndCate.js 脚本
function DimIndCateRunPage() {




    options = {

        renderDiv: $("#DimIndCateTree"),
        hidId: $("#hidId"),
        frmElement: $("#frmDimIndCate"),



        UpdateBtn: $("#btnDimIndCateUpdate"),
        AddBtn: $("#btnDimIndCateAdd"),
        DelBtn: $("#btnDimIndCateDel"),


        //节点数据
        NodeValue: ko.observable({

            Id: 0,
            SCateName: '',
            ILibID: 0,
            IParentID: 0,
            ILevel: 0,
            ILeaf: 0,
            SCateIntro: '',
            SCateAllName: '',
            SMemo: '',
            SCateCode: ''

        }),

        //用户查询URL
        dataQueryUrlAccessor: function () { return "/WebApi/DimIndCate/DimLibIndicatorTree?libId=" + $("#ILibID").val(); },

        //添加用户URL
        dataAddUrlAccessor: function (key) {
            return "/WebApi/DimIndCate/AddTreeNode/" + key;
        },
        dataUpdateAccessor: function (key) {
            //设置当前分类的库ID为选择的库的Id
            options.NodeValue().ILibID = $("#ILibID").val();
            return "/WebApi/DimIndCate/Put/" + key;
        },
        dataDeleteAccessor: function (key) {


            return "/WebApi/DimIndCate/DelTreeNode/" + key;

        },
        dataNodeSelect: function (id) {
            return "/WebApi/DimIndCate/get/" + id;

        }

           , showDlgBefore: function () {
               //显示对话框之前

           }
           , showDetailEnd: function (data) {
               viewModel.judgeButtonStatus();

           }
            , changeEnd: function (key, event) {
                //对话框保存的后置事件

            }, initEd: function () {
                //gird初始化的后置事件 信息

            }

    };

    viewModel = new ViewModel(options);

    viewModel.init();

    //add your  knockout  function  here




    //判断"查看全部指标"按钮的状态
    viewModel.judgeButtonStatus = function () {

        //如果当前节点是叶子节点
        if (options.NodeValue().ILeaf == 1) {
            $("#btnDimIndCateIndicators").removeClass('disabled');
        } else {

            $("#btnDimIndCateIndicators").addClass('disabled');
        }

    }


    //点击按钮，查看具体的指标
    viewModel.seeDeatail = function () {

        var cateId = options.NodeValue().Id;
        
        window.location.hash = "/EconomicData/Indicator?cateId=" + cateId + "&libId=" + $("#ILibID").val();

    }


    ko.applyBindings(viewModel, c1);


    viewModel.judgeButtonStatus();




    options.frmElement.validate({
        rules: {


            txtbId: { required: true, digits: true },
            txtbSCateName: { maxlength: 50 },
            txtbILibID: { digits: true },
            txtbIParentID: { digits: true },
            txtbILevel: { digits: true },
            txtbILeaf: { digits: true },
            txtbSCateIntro: { maxlength: 50 },
            txtbSCateAllName: { maxlength: 50 },
            txtbSMemo: { maxlength: 50 },
            txtbSCateCode: { maxlength: 50 },




        },
        messages: {


            txtbId: { required: '必填项', digits: '必须是整数' },
            txtbSCateName: { maxlength: '最大长度为200' },
            txtbILibID: { digits: '必须是整数' },
            txtbIParentID: { digits: '必须是整数' },
            txtbILevel: { digits: '必须是整数' },
            txtbILeaf: { digits: '必须是整数' },
            txtbSCateIntro: { maxlength: '最大长度为2000' },
            txtbSCateAllName: { maxlength: '最大长度为4000' },
            txtbSMemo: { maxlength: '最大长度为500' },
            txtbSCateCode: { maxlength: '最大长度为20' },


        }
    });


    $("#ILibID").change(function () {
        viewModel.init();
    });

};