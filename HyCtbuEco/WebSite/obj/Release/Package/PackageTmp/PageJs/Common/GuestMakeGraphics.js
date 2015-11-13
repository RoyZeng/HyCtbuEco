function GuestMakeGraphicsRunPage() {


    var libId = $("#hiddenLibId").val();



    var infoData = []; //坐标轴信息
    var valueData = [];  //值信息
    var legendData = [];  //标示信息

    var modeType = "line";  //查看模式,默认为“line”


    var globalData = null;  //保存从数据库中获取的数据




    $('.select2-default').select2();



    options = {};

    gdViewModel = new gridViewModel(options);


    //add your gdViewModel function here



    //返回主页
    gdViewModel.backToHomePage = function () {
        //返回到主页
        window.location.hash = "/Guest/Home?libId";
        
    }



    gdViewModel.DealData = function (data) {


        if (data != null) {
            infoData = [];  //清空横轴信息
            valueData = []; //清空纵轴信息
            legendData = []; //清空说明信息
            for (var i = 0; i < data.timeNameList.length; i++) {
                infoData[i] = data.timeNameList[i]; //横轴时间名称

            }


            for (var i = 0; i < data.dataList.length; i++) {

                legendData[i] = data.dataList[i][0].SIndName; //标示名称
                var tempArray = data.dataList[i];

                var tempData = [];

                for (var j = 0; j < tempArray.length; j++) {
                    tempData[j] = tempArray[j].FValue;
                }

                var tempObj = {
                    "name": legendData[i],
                    "type": modeType,
                    "data": tempData,
                    markPoint: {
                        data: [
                            { type: 'max', name: '最大值' },
                            { type: 'min', name: '最小值' }
                        ]
                    },
                    markLine: {
                        data: [
                            { type: 'average', name: '平均值' }
                        ]
                    }
                }
                valueData[i] = tempObj;


            }
            gdViewModel.InitECharts();


        }

    }



    //初始化echarts.js
    gdViewModel.InitECharts = function () {
        require.config({
            paths: {
                echarts: '/Scripts/echarts/build/dist'
            }
        });
        // 使用
        require(
            [
                'echarts',
                'echarts/chart/bar',// 使用柱状图就加载bar模块，按需加载
                'echarts/chart/line',  //折线图line模块
            ],
            function (ec) {
                // 基于准备好的dom，初始化echarts图表
                var myChart = ec.init(document.getElementById('barChart'));

                var option = {
                    title: {
                        text: '数据结果:',
                    },
                    tooltip: {
                        show: true,
                        trigger: 'axis'
                    },
                    legend: {
                        data: legendData
                    },
                    toolbox: {
                        show: true,
                        feature: {
                            mark: { show: false },
                            dataView: { show: false, readOnly: true },
                            magicType: { show: true, type: ['line', 'bar'] },
                            restore: { show: true },
                            saveAsImage: { show: false }
                        }
                    },
                    calculable: true,
                    xAxis: [
                        {
                            type: 'category',
                            data: infoData
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value'
                        }
                    ],
                    series: valueData
                };

                // 为echarts对象加载数据 
                myChart.setOption(option);
            }
        );

        $("#s2id_ChosenIndicator").removeClass('form-control');
    }



    //查询数据
    gdViewModel.SearchData = function () {

        //获取已选的指标
        var selectedIndicators = $("#ChosenIndicator").val();

        //获取要查看的地区

        var selectedAreas = $("#ChosenArea").val();

        //获取选择查看的模式
        var selectedModel = $("#ChosenModel").val();

        //判断是否有指标被选中
        if (selectedIndicators != null) {

            $.ajax({
                url: '/WebApi/GuestAllData/SearchData',
                type: 'get',
                dataType: "json",
                traditional: true,
                data: { libId: libId, "indiactorArray": selectedIndicators, areaId: selectedAreas },
                success: function (data) {

                    if (data != null) {
                        globalData = data;
                        gdViewModel.DealData(globalData);
                    }

                }

            });

        } else {
            alert("请至少选择一个指标");
        }
    }





    //监听模式选项的change事件
    $("#ChosenModel").on('change', function () {
        var selectModel = $(this).val();  //选择要查看的模式
        switch (selectModel) {
            case "1":
                modeType = 'line';
                break;
            case "2":
                modeType = 'bar';
        }
        gdViewModel.DealData(globalData);
    })



    gdViewModel.SearchData();

    ko.applyBindings(gdViewModel, c1);

}