<template>
  <div class="chart" ref="chart"></div>
</template>

<script>
  import { createChart } from 'lightweight-charts';
  import * as numeral from 'numeral';
  import * as moment from 'moment';

  export default {
        name: "tv-lw-chart",
        props: ['dataName', 'payload', 'height', 'fitContent', 'intradayData', 'showGrid', 'showTimeScale', 'lockTimeScale',
          'showPriceScale', 'legend', 'magnetTip'],
        data: function () {
          return {
          }
        },
        mounted() {
          const chartName = this.dataName;

          if (this.height) {
            this.$refs.chart.style.height = this.height;
          }

          // Chart setup
          let chart = createChart(this.$refs.chart, {
            width: this.$refs.chart.offsetWidth,
            height: this.$refs.chart.offsetHeight
          });

          if (this.intradayData) {
            chart.applyOptions({
              timeScale: {
                timeVisible: true,
                secondsVisible: true,
              }
            });
          }

          let areaSeries = chart.addAreaSeries();
          areaSeries.setData(this.payload);

          chart.timeScale().setVisibleRange(chart.timeScale().getVisibleRange());

          if (this.fitContent) {
            chart.timeScale().fitContent();
          }

          if (!this.showGrid) {
            chart.applyOptions({
              grid: {
                vertLines: {
                  visible: false,
                },
                horzLines: {
                  visible: false,
                },
              }
            })
          }

          if (!this.showTimeScale) {
            chart.applyOptions({
              timeScale: {
                visible: false,
              }
            })
          }

          if (this.lockTimeScale) {
            chart.applyOptions({
              timeScale: {
                fixLeftEdge: true,
                lockVisibleTimeRangeOnResize: true,
              }
            });
          }

          if (!this.showPriceScale) {
            chart.applyOptions({
              priceScale: {
                position: 'none',
                borderVisible: false,
              }
            });
          }

          if (this.legend) {
            let toolTipMargin = 10;
            let priceScaleWidth = 50;
            let toolTip = document.createElement('div');
            toolTip.className = 'three-line-legend';
            this.$refs.chart.appendChild(toolTip);
            toolTip.style.display = 'block';
            toolTip.style.left = 3 + 'px';
            toolTip.style.top = 3 + 'px';

            // Runtime props
            let currPayload = this.payload;
            let width = this.$refs.chart.offsetWidth;
            let height = this.$refs.chart.offsetHeight;

            function setLB(toolTip) {
              toolTip.innerHTML =	'<div style="font-size: 24px; margin: 4px 0px; color: #20262E"> ' + chartName + '</div>'+
                '<div style="font-size: 22px; margin: 4px 0px; color: #20262E">' + numeral(currPayload[currPayload.length-1].value).format('$0,00') + '</div>' +
                '<div>' + moment.unix(param.time).format("MMMM Do YYYY, h:mm:ss a") + '</div>';
            }

            chart.subscribeCrosshairMove(function(param) {
              // When mouse is not here
              if (param === undefined || param.time === undefined || param.point.x < 0
                || param.point.x > width || param.point.y < 0 || param.point.y > height ) {
                setLB(toolTip);
              } else {
                let price = param.seriesPrices.get(areaSeries);
                toolTip.innerHTML =
                  '<div style="font-size: 24px; margin: 4px 0px; color: #20262E"> '
                  + chartName +
                  '</div>'+
                  '<div style="font-size: 22px; margin: 4px 0px; color: #20262E">'
                  + numeral(price).format('$0,00') +
                  '</div>' +
                  '<div>' + param.time + '</div>';
              }
            });
          }

          if (this.magnetTip) {
            let toolTipWidth = 100;
            let toolTipHeight = 80;
            let toolTipMargin = 15;

            let toolTip = document.createElement('div');
            toolTip.className = 'floating-tooltip-2';
            this.$refs.chart.appendChild(toolTip);

            // Runtime props
            let currPayload = this.payload;
            console.dir(currPayload);
            let width = this.$refs.chart.offsetWidth;
            let height = this.$refs.chart.offsetHeight;

            // update tooltip
            chart.subscribeCrosshairMove(function(param) {
              if (!param.time || param.point.x < 0 || param.point.x > width || param.point.y < 0 || param.point.y > height) {
                toolTip.style.display = 'none';
                return;
              }

              toolTip.style.display = 'block';
              let price = param.seriesPrices.get(areaSeries);
              toolTip.innerHTML = '<div style="color: rgba(255, 70, 70, 1)">' + chartName + '</div>' +
                '<b style="font-size: 24px; margin: 4px 0px">' + numeral(price).format('$0.00 a') + '</b>' +
                '<div>' + moment.unix(param.time).format("MMMM Do YYYY, h:mm:ss a") + '</div>';

              let y = param.point.y;

              let left = param.point.x + toolTipMargin;
              if (left > width - toolTipWidth) {
                left = param.point.x - toolTipMargin - toolTipWidth;
              }

              let top = y + toolTipMargin;
              if (top > height - toolTipHeight) {
                top = y - toolTipHeight - toolTipMargin;
              }

              toolTip.style.left = left + 'px';
              toolTip.style.top = top + 'px';
            });
          }

          // Chart watermarking
          chart.applyOptions({
            priceScale: {
              autoScale: true,
              scaleMargins: {
                top: 0.1,
                bottom: 0.1,
              },
            },
            timeScale: {
              lockVisibleTimeRangeOnResize: true,
              rightBarStaysOnScroll: true,
              borderVisible: false,
              borderColor: '#fff000',
              timeVisible: true,
              secondsVisible: false,
            }
          });
        }
    }
</script>

<style scoped>
  .chart {
    width: 100%;
    height: 50vh;
  }
</style>
