
//Write by tms.2015.8.22
//example:var my1=$("#spin1").tmsspinner();
//var curV=my1.getValue();
(function ($) {
    $.fn.tmsspinner = function (options) {
        var defaults = {
            low: 0,
            high: 10
        };
        var opts = $.extend(defaults, options);

        var container = $(this);

        var btnup = container.find(".btnup");
        var btndown = container.find(".btndown");
        var btnText = container.find(".spinText");
        btnup.click(function (a, b, c) {
            var curT = btnText.html();
            curT = parseInt(curT);
            if (curT < opts.high) {
                curT++;
                btnText.html(curT);
            }

            if (opts.clickOver) {
                opts.clickOver();

            }

        });

        btndown.click(function (a, b, c) {
            var curT = btnText.html();
            curT = parseInt(curT);
            if (curT > opts.low) {
                curT--;
                btnText.html(curT);
            }

            if (opts.clickOver) {
                opts.clickOver();

            }
        });

        this.getValue = function () {
            var curT = btnText.html();
            curT = parseInt(curT);
            return curT;
        }

        this.setValue = function (value) {
            if (value < opts.low) {
                value = low;
            } else if (value > opts.high) {
                value = high;
            }
            
            btnText.html(value);
           
            return false;
        }


        return this;
    };



})(jQuery);


