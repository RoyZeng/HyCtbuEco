//开始DimLib.js 脚本
function DimLibRunPage() {


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

        dialogId: "DimLibMessageModal", //添加对话框

        pageSize: 15, //分页大小
        hidId: $("#txtbID"),
        SaveUpdateBtn: $("#btnSaveOrUpdateModel"),
        frmElement: $("#formDimLib"),

        //节点数据
        NodeValue: ko.observable({

            Id: 0,
            SLibName: '',
            STableName: '',
            DCreate: new Date(),
            SIntro: '',
            SMemo: '',


        }),


        //用户列表Grid头部列名称
        headers: [
            { displayText: '库名', value: 'SLibName', width: 'auto' },
            { displayText: '表名', value: 'STableName', width: 'auto' },
            { displayText: '建立时间', value: 'DCreate', width: 'auto' },
        ],

        //默认的排序方式
        defaultOrderBy: "ID",
        //用户查询URL
        dataQueryUrlAccessor: function () {
            return "/WebApi/DimLib/GetByPage";
        },
        AddBefore: function () {
            //表单发送前的函数
            return true;
        },

        //添加用户URL
        dataAddUrlAccessor: function () { return "/WebApi/DimLib/Insert"; },
        dataUpdateAccessor: function (key) { return "/WebApi/DimLib/Update/" + key; },
        dataNodeSelect: function (id) {
            return "/WebApi/DimLib/get/" + id;
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
                return appendQueryString("/WebApi/DimLib/DeleteIds", { ids: userIDs });
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



    //验证库名是否可用
    $("#txtbSLibName").on("blur", function (e) {

        var libName = $(this).val();
        if ($.trim(libName) != null) {
            $.ajax({
                url: "/WebApi/DimLib/ValidateLibName",
                data: { libName: libName },
                success: function (data) {
                    //说明已存在同名的库，库名不合法
                    if (data == 1) {
                        alert("已存在同名库，请从新命名！");
                    }
                }
            })
        }

    });


    //验证表名是否可用

    $("#txtbSTableName").on("blur", function () {

        var tableName = $(this).val();
        if ($.trim(tableName) != null) {
            $.ajax({
                url: "/WebApi/DimLib/ValidateTableName",
                data: { tableName: tableName },
                success: function (data) {
                    //说明已存在同名的库，库名不合法
                    if (data == 1) {
                        alert("已存在同名库，请从新命名！");
                    }
                }
            })
        }

    })



    gdViewModel.init();
    ko.applyBindings(gdViewModel, c1);


    options.frmElement.validate({
        rules: {


            txtbId: { required: true, digits: true },
            txtbSLibName: { required: true, maxlength: 25 },
            txtbSTableName: { required: true, maxlength: 25 },
            txtbDCreate: { date: true },
            txtbSIntro: { maxlength: 100 },
            txtbSMemo: { maxlength: 100 }



        },
        messages: {


            txtbId: { required: '必填项', digits: '必须是整数' },
            txtbSLibName: { maxlength: '最大长度为25', required: '必填项' },
            txtbSTableName: { maxlength: '最大长度为25', required: '必填项' },
            txtbDCreate: { date: '必须是日期格式，例如：2013/01/01' },
            txtbSIntro: { maxlength: '最大长度为100' },
            txtbSMemo: { maxlength: '最大长度为100' }

        }
    });







};





