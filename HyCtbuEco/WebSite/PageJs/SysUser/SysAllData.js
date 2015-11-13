
function SysAllDataRunPage() {

    //判断$("#dt_basic")是否被初始化
    var isDt_basicInit = false;

    //判断$("#dt_basic_format")是否被初始化
    var isDt_basic_formatInit = false;


    //全局数组，保存参数类型
    var typeArray = ["indicatorType", "areaType", "timeType"];

    var selectedIndicator = $("#selectedIndicator");

    options = {};
    viewModel = new ViewModel(options);
    //监听指标的事件
    viewModel.indicatorCheck = function (a, b, c, d) {

        //获取当前节点的子节点
        var selNode = $(a);
        var subNodes = selNode.children("input[name='checkbox']");
        //获取当前节点的ID值并去除前缀“indicatorId”
        var indicatorId = selNode.attr("id").substring(11);
        //获取当前节点的文本内容
        var text = selNode.text();
        subNodes.each(function () {
            //判断当前节点是否被勾选

            //如果是，检查已选指标栏是否已有该指标，如果没有，就添加
            if ($(this).prop("checked")) {
                var selectedIndicatorNodes = selectedIndicator.children();
                var flag = false;
                selectedIndicatorNodes.each(function () {
                    //获取id值并去除前缀“selectedIndicatorId”
                    var selectedId = $(this).attr("id").substring(19)
                    //说明在已选指标栏中存在该指标
                    if (indicatorId == selectedId) {
                        flag = true;
                    }
                });
                if (flag == false) {
                    //创建DOM节点，并添加到该DOM树中,并设置为checked
                    //重组id串,为他添加前缀selectedIndicatorId
                    var id = "selectedIndicatorId" + indicatorId;

                    var newNode = "<label class='checkbox' id='" + id + "' onclick='viewModel.UnInDicatorCheck(this)'><input type='checkbox' checked='checked'  name='checkbox'><i></i>" + text + "</label>";
                    $("#selectedIndicator").append(newNode);
                }
            }
                //没有勾选，判断已选指标栏中是否存在该指标，如果存在，移除
            else {
                var selectedIndicatorNodes = $("#selectedIndicator").children();
                selectedIndicatorNodes.each(function () {
                    //获取id值并去除前缀“selectedIndicatorId”
                    var selectedId = $(this).attr("id").substring(19)
                    //说明在已选指标栏中存在该指标,移除
                    if (indicatorId == selectedId) {
                        $(this).remove();
                    }

                })
            }
        });


    }



    //监听地区的事件
    viewModel.AreaCheck = function (a, b, c, d) {

        //获取当前节点的子节点
        var selNode = $(a);
        var subNodes = selNode.children("input[name='checkbox']");
        if ($(subNodes[0]).prop("checked")) {
            selNode.remove();

            selNode.remove();
            //设置反选

            //取当前节点的Id并去掉前缀"selectedAreaId"
            var selectedAreaId = selNode.attr('id').substring(14);
            var treeNode = viewModel.zTreeObj.getNodeByParam("id", selectedAreaId, null); //如果有高度节点，则选中它
            viewModel.zTreeObj.checkNode(treeNode, false, false, false);//取消勾选节点
        }



    }


    //监听指标的事件
    viewModel.TimeCheck = function (a, b, c, d) {

        //获取当前节点的子节点
        var selNode = $(a);
        var subNodes = selNode.children("input[name='checkbox']");
        if ($(subNodes[0]).prop("checked")) {
            selNode.remove();
            //设置反选
            //取当前节点的Id并去掉前缀"selectedTimeId"
            var selectedTimeId = selNode.attr('id').substring(14);
            var treeNode = viewModel.zTreeObj.getNodeByParam("id", selectedTimeId, null); //如果有高度节点，则选中它
            viewModel.zTreeObj.checkNode(treeNode, false, false, false);//取消勾选节点

        }

    }



    viewModel.UnInDicatorCheck = function (a, b, c, d) {
        var selNode = $(a);
        var subNodes = selNode.children("input[name='checkbox']");
        if ($(subNodes[0]).prop("checked")) {
            selNode.remove();
        }
        var indicatorId = selNode.attr('id').substring(19);
        var checkboxNode = $("#selectableIndicator").children();
        checkboxNode.each(function () {
            var tempId = $(this).attr('id').substring(11);
            if (tempId == indicatorId) {
                $($(this).children()[0]).prop("checked", false);
            }
        })
    }







    //参数初始化事件
    viewModel.ParamInit = function () {

        //获取指标分类Tree数据并显示，只显示当前库的所有指标分类
        $.ajax({
            type: "post",
            url: "/WebApi/DimIndCate/DimLibIndicatorTreClose",
            data: { libId: $("#txtbSrSLibName").val() }
        }).done(function (data) {
            var setting = {
                view: {
                    showIcon: true
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                },
                callback: {
                    onClick: function (event, treeId, treeNode) {
                        //获取对应的指标详细，并且加入到可选指标栏
                        $.ajax({
                            url: "/WebApi/SysAllData/GetIndicatorByCateId",
                            type: "post",
                            data: { cateId: treeNode.id },
                            success: function (data) {
                                if (data != null) {
                                    $("#selectableIndicator").empty();
                                    var tempId = "";  //临时变量
                                    var tempText = ""; //临时变量
                                    for (var i = 0; i < data.length; i++) {
                                        tempId = "indicatorId" + data[i].Id;
                                        tempText = data[i].SIndName;
                                        var newNode = "<label class='checkbox' id='" + tempId + "' onclick='viewModel.indicatorCheck(this)'><input type='checkbox'  name='checkbox'><i></i>" + tempText + "</label>"
                                        $("#selectableIndicator").append(newNode);

                                    }


                                }

                            }
                        })
                    }
                }
            };




            //打开指标树

            var treeNodes = data;
            viewModel.zTreeObj = $.fn.zTree.init($("#Indicattree"), setting, treeNodes);
        });



        //获取地区Tree数据并显示，由于关联了库，需要修改查询方法
        $.ajax({
            type: "post",
            url: "/WebApi/DimArea/DimLibAreaTree",
            data: { libId: $("#txtbSrSLibName").val() }
        }).done(function (data) {
            var setting = {
                view: {
                    showIcon: true
                },
                check: {
                    enable: true,
                    chkboxType: { "Y": "", "N": "" }  //勾选时不关联
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                },
                callback: {
                    onCheck: function (event, treeId, treeNode) {
                        var subNodes = $("#selectedArea").children();
                        var flag = false;
                        if (typeof subNodes != 'undefined') {
                            subNodes.each(function () {

                                var id = $(this).attr('id').substring(14);
                                if (treeNode.id == id) {
                                    flag = true;
                                }
                            })
                        }


                        //如果勾选根节点，不将其加入到已选地区栏
                        if (treeNode.id > 0) {


                            //如果当前节点被选中，检查在已选时间栏是否含有，如果没有，添加
                            if (treeNode.checked) {
                                if (flag == false) {
                                    var lableId = "selectedAreaId" + treeNode.id;

                                    var newNode = "<label class='checkbox' id='" + lableId + "' onclick='viewModel.AreaCheck(this)'><input type='checkbox' checked='checked'  name='checkbox'><i></i>" + treeNode.name + "</label>";
                                    $("#selectedArea").append(newNode);
                                }
                            }
                                //说明当前节点没有被选中
                            else {

                                if (flag == true) {
                                    subNodes.each(function () {

                                        var id = $(this).attr('id').substring(14);
                                        if (treeNode.id == id) {
                                            $(this).remove();
                                        }
                                    })
                                }

                            }
                        }
                    }
                }
            };
            //打开地区树

            var treeNodes = data;
            viewModel.zTreeObj = $.fn.zTree.init($("#Areatree"), setting, treeNodes);
        });




        //获取时间Tree数据并显示,由于关联了库，需要修改查询方法
        $.ajax({
            type: "post",
            url: "/WebApi/DimTime/DimLibTimeTree",
            data: { libId: $("#txtbSrSLibName").val() }
        }).done(function (data) {
            var setting = {
                view: {
                    showIcon: true
                },
                check: {
                    enable: true,
                    chkboxType: { "Y": "", "N": "" }  //勾选时不关联
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                },
                callback: {
                    onCheck: function (event, treeId, treeNode) {

                        var subNodes = $("#selectedTime").children();
                        var flag = false;
                        if (typeof subNodes != 'undefined') {
                            subNodes.each(function () {

                                var id = $(this).attr('id').substring(14);
                                if (treeNode.id == id) {
                                    flag = true;
                                }
                            })
                        }


                        if (treeNode.id > 0) {

                            //如果当前节点被选中，检查在已选时间栏是否含有，如果没有，添加
                            if (treeNode.checked) {
                                if (flag == false) {
                                    var lableId = "selectedTimeId" + treeNode.id;

                                    var newNode = "<label class='checkbox' id='" + lableId + "' onclick='viewModel.TimeCheck(this)'><input type='checkbox' checked='checked'  name='checkbox'><i></i>" + treeNode.name + "</label>";
                                    $("#selectedTime").append(newNode);
                                }
                            }
                                //说明当前节点没有被选中
                            else {

                                if (flag == true) {
                                    subNodes.each(function () {

                                        var id = $(this).attr('id').substring(14);
                                        if (treeNode.id == id) {
                                            $(this).remove();
                                        }
                                    })
                                }

                            }
                        }
                    }
                }

            };
            //打开时间树

            var treeNodes = data;
            viewModel.zTreeObj = $.fn.zTree.init($("#Timetree"), setting, treeNodes);
        });

    }


    //初始化左边的树结构
    viewModel.ParamInit();



    ko.applyBindings(viewModel, c1);



    //监听库表的onchange事件
    $("#txtbSrSLibName").on("change", function () {


        //调用初始化左边树结构的事件
        viewModel.ParamInit();

    })


    //可选指标栏全选的实现
    viewModel.IndicatSelectAll = function () {

        var checkboxNode = $("#selectableIndicator").children().children("input[name='checkbox']");

        checkboxNode.each(function () {
            $(this).prop("checked", true);



        })

        //将所有的节点添加到已选指标栏

        var selectedNodes = $("#selectableIndicator").children(); //获取已被勾选的指标

        if (selectedNodes != null) {

            selectedNodes.each(function () {

                //获取当前节点的ID
                var NodeId = $(this).attr("id");

                //获取文本内容

                var NodeText = ($(this).text());
                //调用公用方法，
                viewModel.AddItemToRight(NodeId, typeArray[0], NodeText)

            })
        }



    }



    //公用方法，将可选参数添加到已选参数栏
    viewModel.AddItemToRight = function (id, type, text) {

        //指标参数
        if (type == typeArray[0]) {

            //遍历已选指标栏是否已存在该ID的指标

            //获取已选指标栏所有的子节点

            var indicatorNodes = $("#selectedIndicator").children();

            //判断标志
            var flag = false;


            //对id进行处理，去掉前缀"indicatorId",只保留ID

            id = id.substring(11);

            indicatorNodes.each(function () {

                var selectedId = $(this).attr('id');
                //对id进行处理，去掉前缀"selectedIndicatorId"
                selectedId = selectedId.substring(19);
                if (selectedId == id) {
                    flag = true;
                }
            })


            //说明已选指标栏不存在该指标
            if (flag == false) {
                //创建DOM节点，并添加到该DOM树中,并设置为checked
                //重组id串,为他添加前缀selectedIndicatorId
                id = "selectedIndicatorId" + id;

                var newNode = "<label class='checkbox' id='" + id + "' ><input type='checkbox' checked='checked'  name='checkbox'><i></i>" + text + "</label>";
                $("#selectedIndicator").append(newNode);
            }


        }


    }


    //可选指标栏全不选的实现
    viewModel.UnIndicatSelectAll = function () {
        var checkboxNode = $("#selectableIndicator").children().children("input[name='checkbox']");

        checkboxNode.each(function () {
            $(this).prop("checked", false);
            //在已选指标栏查找是否含有当前ID的指标，如果有，移除
            var indicatorId = $(this).parent().attr('id');
            //去掉前缀“indicatorId”
            indicatorId = indicatorId.substring(11);
            var selectedIndicatorNodes = $("#selectedIndicator").children();
            selectedIndicatorNodes.each(function () {
                //获取id值并去除前缀“selectedIndicatorId”
                var selectedId = $(this).attr("id").substring(19)
                //说明在已选指标栏中存在该指标，移除
                if (indicatorId == selectedId) {
                    $(this).remove();
                }
            })

        })


    }















    //已选指标栏清空的实现

    viewModel.DeleteSelectIndicator = function () {

        $("#selectedIndicator").empty();
        viewModel.UnIndicatSelectAll();

    }



    //已选地区栏清空的实现

    viewModel.DeleteSelectedArea = function () {
        $("#selectedArea").empty();
    }



    //已选时间栏清空的实现

    viewModel.DeleteSelectedTime = function () {

        $("#selectedTime").empty();

    }


    //点击菜单栏的“显示参数”图标执行的方法
    viewModel.executeIndicatParam = function () {

        //添加选中的样式
        $("#executeIndicatParam").addClass('myactive');

        //移除菜单栏其他块的选中样式
        $("#executeIndicatData").removeClass('myactive');
        $("#executeFormatRowsAndCols").removeClass('myactive');
        $("#executeGraphics").removeClass('myactive');
        //显示参数块
        $("#indicatParam").show();

        //隐藏其他三个块
        $("#indicatData").hide();  //隐藏数据块
        $("#indicatDataFormat").hide();  //隐藏行列转换块
        $("#indicatGraphics").hide();  //隐藏作图块块


    }




    //点击菜单栏的“显示数据”执行的方法
    viewModel.executeIndicatData = function () {
        //添加选中的样式
        $("#executeIndicatData").addClass('myactive');

        //移除菜单栏其他块的选中样式
        $("#executeIndicatParam").removeClass('myactive');
        $("#executeFormatRowsAndCols").removeClass('myactive');
        $("#executeGraphics").removeClass('myactive');

        //显示数据块
        $("#indicatData").show();

        //隐藏其他三个块
        $("#indicatParam").hide();  //隐藏参数块
        $("#indicatDataFormat").hide();  //隐藏行列转换块
        $("#indicatGraphics").hide();  //隐藏作图块块


        //获取已选指标，已选地区以及已选时间
        var params = viewModel.getAllSelectedParam();
        //说明没有选择指标
        if (params.IndicatorIds.length <= 0) {
            alert("出错啦，请至少选择一个指标参数");
            viewModel.executeIndicatParam();
            return;
        } else if (params.AreaIds.length <= 0) {
            alert("出错啦，请至少选择一个地区参数");
            viewModel.executeIndicatParam();
            return;
        } else if (params.TimeIds.length <= 0) {
            alert("出错啦，请至少选择一个时间参数");
            viewModel.executeIndicatParam();
            return;
        } else {

            //执行ajax操作，获取数据
            var libID = $("#txtbSrSLibName").val();

            $.ajax({
                url: '/WebApi/SysAllData/QueryData',
                type: "post",
                dataType: "json",
                traditional: true,
                data: { libId: libID, "indiactorArray": params.IndicatorIds, "areaArray": params.AreaIds, "timeArray": params.TimeIds },
                success: function (data) {
                    //TODO
                    if (data != null) {

                        //第一行表头
                        var firstTableHeadTr0 = $("#firstTableHeadTr0");
                        firstTableHeadTr0.empty();
                        firstTableHeadTr0.append("<th rowspan='2' data-class='expand'>时间/(指标、地区)</th>")

                        //处理表头第一行信息
                        var thNode = "";
                        var AreaLength = data.AreaNameList.length;
                        var Indicatorlength = data.IndicatorNameList.length;
                        for (var i = 0; i < data.IndicatorNameList.length; i++) {

                            thNode = "<th data-class='expand' colspan=" + AreaLength + "><i class=' text-muted hidden-md hidden-sm hidden-xs'></i>" + data.IndicatorNameList[i] + "</th>";

                            firstTableHeadTr0.append(thNode);
                        }


                        //第二行表头

                        var firstTableHeadTr1 = $("#firstTableHeadTr1");
                        firstTableHeadTr1.empty();
                        for (var i = 0; i < Indicatorlength; i++) {

                            for (var j = 0; j < AreaLength; j++) {
                                thNode = "<th data-hide='phone'><i class=' text-muted hidden-md hidden-sm hidden-xs'></i>" + data.AreaNameList[j] + "</th>";

                                firstTableHeadTr1.append(thNode);
                            }

                        }


                        //表格内容
                        var mymodel = new Object();
                        mymodel.rows = new Array();
                        var dsLen = data.dataList.length;
                        for (var mm = 0; mm < dsLen; mm++) {
                            var rowItem = new Object();
                            rowItem.cols = new Array();
                            rowItem.cols.push({ FValue: data.dataList[mm][0].STimeName });
                            rowItem.cols = rowItem.cols.concat(data.dataList[mm]);
                            mymodel.rows.push(rowItem);
                        }

                        //var data = {
                        //    title: '基本例子',
                        //    isAdmin: true,
                        //    rows: [{ X:[ '文艺','xxx'] }, {X:['博客','yy']}]
                        //};
                        //var tmpstr = template('TbodyTemplate', data);

                        var tmpstr = template('TbodyTemplate', mymodel);
                        $("#firstTableBody").empty();

                        $("#firstTableBody").append(tmpstr);


                        if (isDt_basicInit == false) {

                            $('#dt_basic').DataTable({

                                //设置DataTable插件的语言栏
                                "oLanguage": {
                                    "sLengthMenu": "每页显示 _MENU_ 条记录",
                                    "sZeroRecords": "抱歉， 没有找到",
                                    "sInfo": "从 _START_ 到 _END_ /共 _TOTAL_ 条数据",
                                    "sInfoEmpty": "没有数据",
                                    "sInfoFiltered": "(从 _MAX_ 条数据中检索)",
                                    "oPaginate": {
                                        "sFirst": "首页",
                                        "sPrevious": "前一页",
                                        "sNext": "后一页",
                                        "sLast": "尾页"
                                    },
                                    "sZeroRecords": "没有检索到数据",
                                }
                            });
                        }




                    }
                }


            })


        }

    }



    //点击“导出”按钮，导出数据到excel
    viewModel.exportToExcel = function () {

        //获取已选指标，已选地区以及已选时间
        var params = viewModel.getAllSelectedParam();


        //执行ajax操作，获取数据
        var libID = $("#txtbSrSLibName").val();


        $.download('/WebApi/SysAllData/PostDataToExcel?libId=' + libID, 'find=commoncode', 'post');




    }









    //获取所有已选的参数
    viewModel.getAllSelectedParam = function () {
        var target = {};

        var IndicatorIds = []; //指标id数组

        var AreaIds = [];  //地区id数组

        var TimeIds = [];  //时间Id数组

        var tempId = 0;

        var TempIndicators = $("#selectedIndicator").children();
        //存在已选指标
        if (TempIndicators.length > 0) {
            TempIndicators.each(function () {
                //获取指标的Id
                tempId = $(this).attr('id').substring(19);
                IndicatorIds.push(tempId);

            })
        }


        var TempAreas = $("#selectedArea").children();
        //存在已选地区
        if (TempAreas.length > 0) {
            TempAreas.each(function () {

                //获取指标的Id
                tempId = $(this).attr('id').substring(14);
                AreaIds.push(tempId);

            });
        }



        var TempTimes = $("#selectedTime").children();

        //存在已选时间
        if (TempTimes.length > 0) {
            TempTimes.each(function () {
                //获取指标的Id
                tempId = $(this).attr('id').substring(14);
                TimeIds.push(tempId);

            })
        }


        target.IndicatorIds = IndicatorIds;
        target.AreaIds = AreaIds;
        target.TimeIds = TimeIds;

        return target;


    }




    //点击菜单栏的“行列转化”执行的方法
    viewModel.executeFormatRowsAndCols = function () {


        //添加选中的样式
        $("#executeFormatRowsAndCols").addClass('myactive');

        //移除菜单栏其他块的选中样式
        $("#executeIndicatData").removeClass('myactive');
        $("#executeIndicatParam").removeClass('myactive');
        $("#executeGraphics").removeClass('myactive');

        //显示行列转化块
        $("#indicatDataFormat").show();

        //隐藏其他三个块
        $("#indicatData").hide();  //隐藏数据块
        $("#indicatParam").hide();  //隐藏参数块
        $("#indicatGraphics").hide();  //隐藏作图块块




        //获取已选指标，已选地区以及已选时间
        var params = viewModel.getAllSelectedParam();
        //说明没有选择指标
        if (params.IndicatorIds.length <= 0) {
            alert("出错啦，请至少选择一个指标参数");
            viewModel.executeIndicatParam();
            return;
        } else if (params.AreaIds.length <= 0) {
            alert("出错啦，请至少选择一个地区参数");
            viewModel.executeIndicatParam();
            return;
        } else if (params.TimeIds.length <= 0) {
            alert("出错啦，请至少选择一个时间参数");
            viewModel.executeIndicatParam();
            return;
        } else {

            //执行ajax操作，获取数据
            var libID = $("#txtbSrSLibName").val();

            $.ajax({
                url: '/WebApi/SysAllData/QueryDataFormat',
                type: "post",
                dataType: "json",
                traditional: true,
                data: { libId: libID, "indiactorArray": params.IndicatorIds, "areaArray": params.AreaIds, "timeArray": params.TimeIds },
                success: function (data) {
                    //TODO
                    if (data != null) {

                        //表头
                        var secondTableHeadTr0 = $("#secondTableHeadTr0");
                        secondTableHeadTr0.empty();
                        var thNode = "";
                        thNode += "<th colspan='2'>(指标、地区)/时间</th>";

                        for (var i = 0; i < data.timeNameList.length; i++) {

                            thNode+= "<th>" + data.timeNameList[i] + "</th>";
                        }
                        secondTableHeadTr0.append(thNode);



                        //表格内容
                        var mymodel = new Object();
                        mymodel.rows = new Array();
                        var dsLen = data.dataList.length;
                        for (var mm = 0; mm < dsLen; mm++) {
                            var rowItem = new Object();
                            rowItem.cols = new Array();
                            rowItem.cols.push({ FValue: data.dataList[mm][0].SIndName });
                            rowItem.cols.push({ FValue: data.dataList[mm][0].SAreaName });
                            rowItem.cols = rowItem.cols.concat(data.dataList[mm]);
                            mymodel.rows.push(rowItem);
                        }


                        var tmpstr = template('TBodyFormatTemplate', mymodel);
                        $("#secontTableBody").empty();

                        $("#secontTableBody").append(tmpstr);

                        if (isDt_basic_formatInit == false) {

                            $('#dt_basic_format').DataTable({
                                //设置DataTable插件的语言栏
                                "oLanguage": {
                                    "sLengthMenu": "每页显示 _MENU_ 条记录",
                                    "sZeroRecords": "抱歉， 没有找到",
                                    "sInfo": "从 _START_ 到 _END_ /共 _TOTAL_ 条数据",
                                    "sInfoEmpty": "没有数据",
                                    "sInfoFiltered": "(从 _MAX_ 条数据中检索)",
                                    "oPaginate": {
                                        "sFirst": "首页",
                                        "sPrevious": "前一页",
                                        "sNext": "后一页",
                                        "sLast": "尾页"
                                    },
                                    "sZeroRecords": "没有检索到数据",
                                }
                            });
                            isDt_basic_formatInit = true;
                        }


                    }
                }


            })


        }



        








    }

    //点击菜单栏的“作图”执行的方法
    viewModel.executeGraphics = function () {

        //显示作图块
        $("#indicatGraphics").show();

        //隐藏其他三个块
        $("#indicatData").hide();  //隐藏数据块
        $("#indicatDataFormat").hide();  //隐藏行列转换块
        $("#indicatParam").hide();  //隐藏参数块

        //添加选中的样式
        $("#executeGraphics").addClass('myactive');

        //移除菜单栏其他块的选中样式
        $("#executeIndicatData").removeClass('myactive');
        $("#executeIndicatParam").removeClass('myactive');
        $("#executeFormatRowsAndCols").removeClass('myactive');

        //获取库Id
        var libID = $("#txtbSrSLibName").val();


        //获取已选指标，已选地区以及已选时间
        var params = viewModel.getAllSelectedParam();
        //说明没有选择指标
        if (params.IndicatorIds.length <= 0) {
            alert("出错啦，请至少选择一个指标参数");
            viewModel.executeIndicatParam();
            return;
        } else if (params.AreaIds.length <= 0) {
            alert("出错啦，请至少选择一个地区参数");
            viewModel.executeIndicatParam();
            return;
        } else if (params.TimeIds.length <= 0) {
            alert("出错啦，请至少选择一个时间参数");
            viewModel.executeIndicatParam();
            return;
        } else {

            $.ajax({
                url: '/WebApi/SysAllData/SaveParamToSession',
                type: "post",
                dataType: "json",
                traditional: true,
                data: { "indiactorArray": params.IndicatorIds, "areaArray": params.AreaIds, "timeArray": params.TimeIds },
                success: function (data) {
                    if (data == 1) {
                        window.location.hash = "/EconomicData/MakeGraphics?libID=" + libID;
                    } else {
                        alert("服务器错误");
                    }
                }
            });






        }

        


    }




}

