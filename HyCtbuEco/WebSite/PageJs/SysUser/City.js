function DimCityRunPage() {
        

    options = {

        renderDiv: $( "#CityTree" ),
        hidId: $( "#hidId" ),
        frmElement: $( "#frmCity" ),



        UpdateBtn: $( "#btnCityUpdate" ),
        AddBtn: $( "#btnCityAdd" ),
        DelBtn: $( "#btnCityDel" ),


        //节点数据
        NodeValue: ko.observable( {
               
            Id: 0, SAddireName: '',
            IParentID: 0,
            ILevel: 0,
            SCode: '',
            SPostCode: ''
            , ISort: 0
						
        } ),
        //用户查询URL
        dataQueryUrlAccessor: function () { return "/WebApi/City/TreeAll"; },

        //添加用户URL
        dataAddUrlAccessor: function ( key ) { return "/WebApi/City/AddTreeNode/" + key; },
        dataUpdateAccessor: function ( key ) { return "/WebApi/City/Put/" + key; },
        dataDeleteAccessor: function ( key ) {


            return "/WebApi/City/DelTreeNode/" + key;

        },
        dataNodeSelect: function ( id ) {
            return "/WebApi/City/get/" + id;
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

    viewModel = new ViewModel( options );


    viewModel.init();
    ko.applyBindings( viewModel ,c1);




    options.frmElement.validate( {
        rules: {

			
            txtbId:{required: true,digits:true},txtbSAddireName:{maxlength: 50},txtbIParentID:{digits:true},txtbILevel:{digits:true},txtbSCode:{maxlength: 50},txtbSPostCode:{maxlength: 50}
						
          
			
        },
        messages: {

                
            txtbId:{required: '必填项',digits:'必须是整数'},txtbSAddireName:{maxlength: '最大长度为50'},txtbIParentID:{digits:'必须是整数'},txtbILevel:{digits:'必须是整数'},txtbSCode:{maxlength: '最大长度为10'},txtbSPostCode:{maxlength: '最大长度为40'}

        }
    } );

} 