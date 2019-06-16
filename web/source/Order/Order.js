var elemnets = {
    ids: ["OrderNo", "PayWay", "TransportNumber", "OrderDetailJson"],
    methods: [
        {
            required: true,
            messages: ["请输入订单号", "", "", ""]
        },
        {
            required: true,
            messages: ["请输入付款方式", "", "", ""]
        },
        {
            required: true,
            messages: ["请输入运单号", "", "", ""]
        },
        {
            required: true,
            messages: ["请输入产品信息", "", "", ""]
        }
    ]
};

var orderInfo = {
    init: function () {
        valid.init(elemnets);
        orderInfo.regEvent();
        orderInfo.initCity();
    },

    regEvent: function () {      

        $("#Province").on("change", function () {
            var $this = $(this);
            if (!!$this.val()) {
                $.ajax({
                    url: "GetCity",
                    type: 'POST',
                    async: false,
                    data: { pid: $this.val() },
                    success: function (data) {
                        if (!!data) {
                            $("#City").html("").append("<option value=''>--城市--</option>");

                            for (var i = 0; i < data.length; i++) {
                                $("#City").append("<option value='" + data[i].CityID + "'>" + data[i].CityName + "</option>");
                            }
                        }
                    }
                });
            }
            else {
                $("#City").html("").append("<option value=''>--城市--</option>");
            }
        });

        $("#save").click(function () {

            var formParams = {};
            var params = {
                orderDetail: []
            };
            $("#parentTbody").find("tr").each(function () {
                var tdArr = $(this).children();

                var author = tdArr.eq(0).find('input').val();//师傅
                var productID = tdArr.eq(1).find('input').val();//产品编号 -
                var productName = tdArr.eq(2).find('input').val();//作品名称 -
                var quantity = tdArr.eq(3).find('input').eq(0).val();//数量                 
                var salePrice = tdArr.eq(4).find('input').val();//售价
                var collectedSalePrice = tdArr.eq(5).find('input').val();//实际卖价
                if (author != "Head") {
                    formParams = {
                        "Author": author,
                        "ProductID": productID,
                        "ProductName": productName,
                        "Quantity": quantity,
                        "SalePrice": salePrice,
                        "CollectedSalePrice": collectedSalePrice,
                    };
                    params.orderDetail.push(formParams);
                }

            });

            $("#OrderDetailJson").val(JSON.stringify(params));

            if (valid.validate()) {
                $("#OrderForm").submit();
            }
        });

       
    },
    initCity: function () {
        var pid = $("#hid_provinceid").val(), cid = $("#hid_cityid").val();
        if (pid != "" && pid != "0") {
            $("#Province").val(pid).trigger("change");
            if (cid != "" && cid != "0") {
                $("#City").val(cid);
            }
        }

    }
}

/**删除一行*/
function delTr($this, id) {
    $($this).parent().parent().remove();
}
$("#goback").click(function () {
    window.location.href = "/Order/";
});


