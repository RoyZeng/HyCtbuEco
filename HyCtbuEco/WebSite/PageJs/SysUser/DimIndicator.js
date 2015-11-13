//开始DimIndicator.js 脚本
function DimIndicatorRunPage() {


    //全局变量，当前指标所属分类Id
    var cateId = $("#txtbSrICateID").val();

    //全局变量,当前指标所属库的Id
    var libId = $("#txtbSrILibID").val();


    //全选的实现
    $("#headcheck").click(function () {
        var x = $('#headcheck').prop("checked");
        if (x == true) {
            $("input[name='userSelector']").each(function () {
                $(this).prop("checked", true);
            });
        }
        else {
            $("input[name='userSelector']").each(function () {
                $(this).prop("checked", false);
            });
        }
    });


    options = {

        dialogId: "DimIndicatorMessageModal", //添加对话框

        pageSize: 15, //分页大小
        hidId: $("#txtbID"),
        SaveUpdateBtn: $("#btnSaveOrUpdateModel"),
        frmElement: $("#formDimIndicator"),

        //节点数据
        NodeValue: ko.observable({

            Id: 0,
            ICateID: 0,
            SCateName:'',
            SIndName: '',
            SIndIntro: '',
            SIndCode: '',
            ILibID: 0,
            SLibName: '',
            SUnit:''  //指标单位


        }),

        //用户列表Grid头部列名称
        headers: [
                     { displayText: 'Id', value: 'Id', width: 'auto' },
                     { displayText: '指标类型名', value: 'SCateName', width: 'auto' },
                     { displayText: '指标名称', value: 'SIndName', width: 'auto' },
                     { displayText: '指标单位', value: 'SUnit', width: 'auto' },
                     { displayText: '指标代码', value: 'SIndCode', width: 'auto' },
                     { displayText: '库名', value: 'SLibName', width: 'auto' }

        ],

        //默认的排序方式
        defaultOrderBy: "ID",
        //用户查询URL
        dataQueryUrlAccessor: function () {
            return "/WebApi/DimIndicator/GetByPage";
        },
        AddBefore: function () {
            //表单发送前的函数
            return true;
        },
        UpdateBefore:function() {
            //更新表单发送前
            return true;
        },

        //添加用户URL
        dataAddUrlAccessor: function () {
            //库的Id
            options.NodeValue().ILibID = libId;
            //指标分类的Id
            options.NodeValue().ICateID = cateId;
            return "/WebApi/DimIndicator/NewPost";
        },
        dataUpdateAccessor: function (key) { return "/WebApi/DimIndicator/Put/" + key; },
        dataNodeSelect: function (id) {
            return "/WebApi/DimIndicator/get/" + id;
        },

        dataDeleteAccessor: function (data) {
            var userIDs = "";
            var check = $("input[name='userSelector']:checked");  //得到所有被选中的checkbox
            check.each(function (i) {        //循环拼装被选中项的值
                userIDs = userIDs + ',' + $(this).val();
            });
            //清除多余的分割号
            userIDs = dropRsplit(userIDs, ",");
            if (userIDs != "") {
                return appendQueryString("/WebApi/DimIndicator/DeleteIds", { ids: userIDs });
            } else
                return "";

        }
        , initEd: function () {
            //gird初始化的后置事件 信息

        }
        , changeEnd: function () {
            //对话框关闭后 的后置事件
        }
        , showDetailEnd: function (data) {
            //显示对话框详细之后
        }, showDlgBefore: function () {
            //对话框打开后，对话框信息显示完成后
        }



    };

    gdViewModel = new gridViewModel(options);


    gdViewModel.init();



    //返回到指标分类页面
    gdViewModel.backToCate = function () {

        window.location.hash = "/EconomicData/IndicatorCate";
    }





    ko.applyBindings(gdViewModel, c1);


    options.frmElement.validate({
        rules: {


            txtbId: { required: true, digits: true },
            txtbICateID: { digits: true },
            txtbSIndName: { required: true, maxlength: 100, validIndName: true },
            txtbSIndIntro: { maxlength: 1000 },
            txtbSIndCode: { required: true, maxlength: 10 },
            txtbILibID: { digits: true }

        },
        messages: {

            txtbId: { required: '必填项', digits: '必须是整数' },
            txtbICateID: { digits: '必须是整数' },
            txtbSIndName: { required: '必填项', maxlength: '最大长度为100', validIndName: '指标名已存在，请重新输入' },
            txtbSIndIntro: { maxlength: '最大长度为1000' },
            txtbSIndCode: { required: '必填项', maxlength: '最大长度为50' },
            txtbILibID: { digits: '必须是整数' }

        }
    });





    var IndNameFlag = true;
    //添加jQuery的验证方法
    jQuery.validator.addMethod("validIndName",
        function (value, element) {
            

            //根据是Id来确定是添加验证还是更新验证
            if (options.NodeValue().Id == 0) {  //添加验证
                //获取指标名称
                var IndName = options.NodeValue().SIndName;
                $.ajax({
                    url: '/WebApi/DimIndicator/ValidateIndName',
                    type: 'post',
                    data: { SIndName: IndName, ICateId: cateId, LibId: libId },
                    success: function (data) {
                        //说明存在
                        if (data == 1) {
                            IndNameFlag = false;
                        }
                    }

                });
            } 
    
            return IndNameFlag;
        }, "指标名已存在，请重新输入");



};