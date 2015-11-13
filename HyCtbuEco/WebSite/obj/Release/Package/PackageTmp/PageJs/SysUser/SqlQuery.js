
//开始SqlQuery.js 脚本
function SqlQueryRunPage() {


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

        dialogId: "SqlQueryMessageModal", //添加对话框

        pageSize: 15, //分页大小
        hidId: $("#txtbID"),
        SaveUpdateBtn: $("#btnSaveOrUpdateModel"),
        frmElement: $("#formSqlQuery"),

        //节点数据
        NodeValue: ko.observable({

            ID: 0,
            SSqlName: '',
            SSqlStr: '',
            ICreateID: 0,
            DCreate: new Date(),
            ISort: 0,


        }),


        //用户列表Grid头部列名称
        headers: [


                     { displayText: 'ID', value: 'Id', width: 'auto' },
{ displayText: 'sql的名称', value: 'SSqlName', width: 'auto' },

{ displayText: '建立者ID', value: 'ICreateID', width: 'auto' },
{ displayText: '创建修改时间', value: 'DCreate', width: 'auto' },
{ displayText: '排序号', value: 'ISort', width: 'auto' }


        ],

        //默认的排序方式
        defaultOrderBy: "ID",
        //用户查询URL
        dataQueryUrlAccessor: function () {
            return "/WebApi/SqlQuery/GetByPage";
        },
        AddBefore: function () {
            //表单发送前的函数
            return true;
        },

        //添加用户URL
        dataAddUrlAccessor: function () { return "/WebApi/SqlQuery/Post"; },
        dataUpdateAccessor: function (key) { return "/WebApi/SqlQuery/Put/" + key; },
        dataNodeSelect: function (id) {
            return "/WebApi/SqlQuery/get/" + id;
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
                return appendQueryString("/WebApi/SqlQuery/DeleteIds", { ids: userIDs });
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
    ko.applyBindings(gdViewModel, c1);


    options.frmElement.validate({
        rules: {


            txtbID: { required: true, digits: true },
            txtbSSqlName: { required: true, maxlength: 50 },
            txtbSSqlStr: { required: true, maxlength: 4000 },
            txtbICreateID: { digits: true },
            txtbDCreate: { date: true }, txtbISort: { digits: true }



        },
        messages: {


            txtbID: { required: '必填项', digits: '必须是整数' }, txtbSSqlName: { required: '必填项', maxlength: '最大长度为25' },
            txtbSSqlStr: { required: '必填项', maxlength: '最大长度为2000' },
            txtbICreateID: { digits: '必须是整数' },
            txtbDCreate: { date: '必须是日期格式，例如：2013/01/01' },
            txtbISort: { digits: '必须是整数' }

        }
    });


};
