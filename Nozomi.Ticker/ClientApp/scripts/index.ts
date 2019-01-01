// https://stackoverflow.com/questions/52376720/how-to-make-font-awesome-5-work-with-webpack
//import fontawesome from '@fortawesome/fontawesome';

import '../styles/index.scss';

import * as $ from 'jquery';
import 'jquery-migrate';
import Typed from 'typed.js';
import 'popper.js';
import 'bootstrap';
import 'slick-carousel';
import 'svg-injector';
import 'malihu-custom-scrollbar-plugin';

// No updated npm version yet
import '../scripts/components/appear.js';

import '../assets/vendor/hs-megamenu/src/hs.megamenu';

import '../scripts/hs.core.js';
import '../scripts/components/hs.bg-video.js';
import '../scripts/components/hs.chart-pie.js';
import '../scripts/components/hs.go-to.js';
import '../scripts/components/hs.malihu-scrollbar.js';
import '../scripts/components/hs.slick-carousel.js';
import '../scripts/components/hs.svg-injector.js';
import '../scripts/components/hs.unfold.js';

$(document).on('ready', function () {
    // initialization of svg injector module
    $.HSCore.components.HSSVGIngector.init('.js-svg-injector');

    // initialization of slick carousel
    $.HSCore.components.HSSlickCarousel.init('.js-slick-carousel');

    // initialization of horizontal progress bars
    var horizontalProgressBars = $.HSCore.components.HSProgressBar.init('.js-hr-progress', {
        direction: 'horizontal',
        indicatorSelector: '.js-hr-progress-bar'
    });

    // initialization of chartist bar chart
    $.HSCore.components.HSChartistBarChart.init('.js-bar-chart');
});