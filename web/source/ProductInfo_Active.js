

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
                    if (infos[i].artisanName != "杨义东") {
                        $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=" + infos[i].artisanName + "'>" + infos[i].artisanName + "</div>");
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
                        $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=" + infos[i].artisanName + "'>" + infos[i].artisanName + "</div>");
                    }
                }
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=吴立主'>吴立主</div>");
            }
        });
        //茶具茶叶
        $("#div_mjgys").on("click", function () {
            var infos = JSON.parse($("#hid_mjgys").val());
            var telephone = $("#hid_telephone").val();
            if (infos != "") {
                $("#div_showinfo").html("");
                //for (var i = 0; i < infos.length; i++) {
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=白印'>白印</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=茶叶'>茶叶</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=绿雪芽'>绿雪芽</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=铁印'>铁印</div>");

                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=鼎炉'>鼎炉</div>");              
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=方几'>方几</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=铜火钵'>铜火钵</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=铜炉'>铜炉</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=锡茶'>锡茶</div>");
                $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&author=锡罐'>锡罐</div>");              
                //}
            }
        });
        //器形/釉色
        $("#div_qxys").on("click", function () {
            var infoqxs = JSON.parse($("#hid_qx").val());
            var infoyss = JSON.parse($("#hid_ys").val());
            var telephone = $("#hid_telephone").val();
            $("#div_showinfo").html("");          
            
            
            $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&type3=束口盏'>束口盏</div>");
            $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&type3=敞口盏/斗笠'>敞口盏/斗笠</div>");
            $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&type3=钵型盏'>钵型盏</div>");
            $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&type3=创新器型'>创新器型</div>");
            $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&type3=敛口盏'>敛口盏</div>");
            $("#div_showinfo").append("<div class='markitem'><a href='/W11/paimai?telephone=" + telephone + "&type3=撇口盏'>撇口盏</div>");

        });

    }
}
