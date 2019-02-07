// https://stackoverflow.com/questions/52376720/how-to-make-font-awesome-5-work-with-webpack
//import fontawesome from '@fortawesome/fontawesome';

import '../styles/index.scss';

import * as $ from 'jquery';
// https://stackoverflow.com/questions/48271232/how-to-expose-ui-jquery-ui-globally-with-webpack
const jQuery = $;
(<any> window).$ = (<any>window).jQuery = $;
import Vue from 'vue';
import BootstrapVue from 'bootstrap-vue';
Vue.use(BootstrapVue); // Expose bootstrap-vue globally
(<any> window).Vue = Vue;
import * as Chartist from 'chartist';
import 'chartist-plugin-tooltips';
const chartist = Chartist;
(<any> window).Chartist = chartist;
import * as signalR from "@aspnet/signalr";
(<any> window).SignalR = signalR;
import 'jquery-validation';
import 'jquery-migrate';
import Typed from 'typed.js';
import 'popper.js';
import 'bootstrap';
import 'bootstrap-select';
import 'slick-carousel';
import 'svg-injector';
import 'malihu-custom-scrollbar-plugin';
import 'datatables';

// No updated npm version yet
import '../scripts/components/appear.js';

import '../assets/vendor/hs-megamenu/src/hs.megamenu';

import '../scripts/hs.core.js';
import '../scripts/components/hs.header.js';
import '../scripts/components/hs.unfold.js';

import '../scripts/components/hs.bg-video.js';
import '../scripts/components/hs.chartist-area-chart.js';
import '../scripts/components/hs.chartist-bar-chart.js';
import '../scripts/components/hs.datatables.js';
import '../scripts/components/hs.focus-state.js';
import '../scripts/components/hs.go-to.js';
import '../scripts/components/hs.malihu-scrollbar.js';
import '../scripts/components/hs.modal-window.js';
import '../scripts/components/hs.progress-bar.js';
import '../scripts/components/hs.show-animation.js';
import '../scripts/components/hs.selectpicker.js';
import '../scripts/components/hs.slick-carousel.js';
import '../scripts/components/hs.step-form.js';
import '../scripts/components/hs.svg-injector.js';
import '../scripts/components/hs.toggle-state.js'
import '../scripts/components/hs.validation.js';
import '../scripts/components/hs.chart-pie.js';
$(window).on('load', function () {
    // initialization of HSMegaMenu component
    $('.js-mega-menu').HSMegaMenu({
        event: 'hover',
        pageContainer: $('.container'),
        breakpoint: 767.98,
        hideTimeOut: 0
    });

    // initialization of HSMegaMenu component
    $('.js-breadcrumb-menu').HSMegaMenu({
        event: 'hover',
        pageContainer: $('.container'),
        breakpoint: 991.98,
        hideTimeOut: 0
    });

    // initialization of svg injector module
    $.HSCore.components.HSSVGIngector.init('.js-svg-injector');
}); 

$(document).on('ready', function () {
    // Optimize scrolling
    $.event.special.touchstart = {
        setup: function( _, ns, handle ){
            if ( ns.includes("noPreventDefault") ) {
                this.addEventListener("touchstart", handle, { passive: false });
            } else {
                this.addEventListener("touchstart", handle, { passive: true });
            }
        }
    };
    
    // initialization of header
    $.HSCore.components.HSHeader.init($('#header'));

    // initialization of svg injector module
    $.HSCore.components.HSSVGIngector.init('.js-svg-injector');

    // initialization of slick carousel
    $.HSCore.components.HSSlickCarousel.init('.js-slick-carousel');

    // initialization of unfold component
    $.HSCore.components.HSUnfold.init($('[data-unfold-target]'), {
        afterOpen: function () {
            $(this).find('input[type="search"]').focus();
        }
    });
    
    // initialization of datatables
    $.HSCore.components.HSDatatables.init('.js-datatable');

    // initialization of malihu scrollbar
    $.HSCore.components.HSMalihuScrollBar.init($('.js-scrollbar'));

    // initialization of forms
    $.HSCore.components.HSFocusState.init();

    // initialization of form validation
    $.HSCore.components.HSValidation.init('.js-validate');

    // initialization of autonomous popups
    $.HSCore.components.HSModalWindow.init('[data-modal-target]', '.js-modal-window', {
        autonomous: true
    });

    // initialization of select picker
    $.HSCore.components.HSSelectPicker.init('.js-select');

    // initialization of step form
    $.HSCore.components.HSStepForm.init('.js-step-form');

    // initialization of show animations
    $.HSCore.components.HSShowAnimation.init('.js-animation-link',
        {
            afterShow: function() {
                $('.js-slick-carousel').slick('setPosition');
            }
        });

    // initialization of chart pies
    let items = $.HSCore.components.HSChartPie.init('.js-pie');
    
    // initialization of horizontal progress bars
    var horizontalProgressBars = $.HSCore.components.HSProgressBar.init('.js-hr-progress', {
        direction: 'horizontal',
        indicatorSelector: '.js-hr-progress-bar'
    });

    var verticalProgressBars = $.HSCore.components.HSProgressBar.init('.js-vr-progress', {
        direction: 'vertical',
        indicatorSelector: '.js-vr-progress-bar'
    });

    // initialization of chartist area charts
    $.HSCore.components.HSChartistAreaChart.init('.js-area-chart');

    // initialization of chartist bar chart
    $.HSCore.components.HSChartistBarChart.init('.js-bar-chart');

    // initialization of go to
    $.HSCore.components.HSGoTo.init('.js-go-to');
});