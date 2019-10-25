

var productinfo = {
    init: function () {
        productinfo.regEvent();
    },

    regEvent: function () {

        //业界大师
        $("#div_yjds").on("click", function () {
            var infos = JSON.parse($("#hid_yjds").val());
            var telephone = $("#hid_telephone").val();
            if (infos != "") {
                $("#div_showinfo").html("");
                for (var i = 0; i < infos.length; i++) {
                    if (infos[i].artisanName != "杨义东"&& infos[i].artisanName != "江有庭") {
                        $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?author=" + infos[i].artisanName + "'>" + infos[i].artisanName + "</div>");
                    }
                }
            }
        });

        //老牌传承人
        $("#div_lpccr").on("click", function () {
            var infos = JSON.parse($("#hid_lpccr").val());
            var telephone = $("#hid_telephone").val();
            if (infos != "") {
                $("#div_showinfo").html("");
                for (var i = 0; i < infos.length; i++) {                                                                                
                    if (infos[i].artisanName != "叶礼旺" && infos[i].artisanName != "叶礼忠" && infos[i].artisanName != "吴立勇" && infos[i].artisanName != "廖设生") {
                        $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?author=" + infos[i].artisanName + "'>" + infos[i].artisanName + "</div>");
                    }
                }
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?author=吴立主'>吴立主</div>");
				$("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?author=张修潘'>张修潘</div>");
            }
        });
        //茶叶茶具
        $("#div_mjgys").on("click", function () {
            var infos = JSON.parse($("#hid_mjgys").val());
            var telephone = $("#hid_telephone").val();
            if (infos != "") {
                $("#div_showinfo").html("");
                //for (var i = 0; i < infos.length; i++) {
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?author=茶叶'>茶叶</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?author=茶具'>茶具</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?author=铁壶'>铁壶</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?author=银壶'>银壶</div>");

                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?author=摆件'>摆件</div>");              
            
                //}
            }
        });
        //器形/釉色
        $("#div_qxys").on("click", function () {

                        var telephone = $("#hid_telephone").val();
            var infoqxs = JSON.parse($("#hid_qx").val());
            var infoyss = JSON.parse($("#hid_ys").val());
            if (infoqxs != "") {
                $("#div_showinfo").html("");
                for (var i = 0; i < infoqxs.length; i++) {
                    $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?type3=" + infoqxs[i].ValueInfo + "'>" + infoqxs[i].ValueInfo + "</div>");
                }
            }
            if (infoyss != "") {
                for (var i = 0; i < infoyss.length; i++) {
                    $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?type2=" + infoyss[i].ValueInfo + "'>" + infoyss[i].ValueInfo + "</div>");
                }
            }
        });

    }
}
