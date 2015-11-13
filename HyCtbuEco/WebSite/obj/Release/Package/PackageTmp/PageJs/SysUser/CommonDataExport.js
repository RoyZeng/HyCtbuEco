                             
function CommonDataExportRunPage() {

   // var gdViewModel;

    var headerlist = new Array;  //表头信息
    var contentlist = new Array; //表内容信息

        function gridViewModel() {
            var self = this;


            self.recordSet = ko.observableArray(); //获取所有的SQL语句

            //初始化
            self.init = function () {
               
            }
            self.initEd = function () {//结束初始化
            }

            //通用数据导出，收集所有的SQL语句名
            self.SearchSQL = function () {

                $.ajax(
               {

                   url: '/api/CommonDataExport/GetSQLName/',
                   type: "get",
                   success: function (result) {
                       if (result == null) {
                           result = "当前数据库不存在SQL查询语句";
                       }

                       self.recordSet(result);//收集返回的数据库表集

                   }
               });
            };



            //TO Add Method  

            //点击“查询数据”  按钮，根据所选的SQL语句，查出对应的数据
            self.QueryData = function () {
                var reslut = $("#selQuerys").val(); //获取所选择的的SQL语句
                $("#tableHeader").empty();  //清空表头
                $.ajax({
                    url: '/WebApi/CommonDataExport/GetSelectedData?id=' + reslut,
                    type: 'POST',
                    success: function (item) {  //把取得的数据放到数据集中  todo
                        $("#ContentId").show();

                        contentlist = item.dataContent; //获取表内容信息
                        headerlist = item.dataHeader; //获取表头信息
                        $.each(headerlist, function (n, value) {

                            $("#tableHeader").append('<td>' + value + '</td')
                        });
                        $("#tableBody").empty();
                        contentlist = item.dataContent; //获取表内容信息
                        $.each(contentlist, function (n, value) {
                            var rowsList = new Array;
                            rowsList = value;
                            var rows = '';
                            $.each(rowsList, function (n, result) {
                                rows = rows + '<td>' + result + '</td>';
                            })
                            $("#tableBody").append('<tr>' + rows + '</tr')

                        });


                    }
                })
            }


            //点击“导出Excel”按钮，根据所选的SQL语句，导出到Excel
            self.PostDataToExcel = function () {
                var reslut = $("#selQuerys").val(); //获取所选择的的SQL语句

                $.download('/WebApi/CommonDataExport/PostDataToExcel?id=' + reslut, 'find=commoncode', 'post');
            }





        }


        var gdViewModel = new gridViewModel();
        gdViewModel.init();
        ko.applyBindings(gdViewModel, c1);


}