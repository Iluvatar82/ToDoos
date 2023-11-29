import "/src/d3/d3.min.js";
import "/jquery/jquery.js";

let svgElement, scaleX, highlighted;

var height = 105;
var marginBottom = 21;
var marginSide = 20;
var eventSize = 5;

export function initializeTimeline()
{
    if (document.querySelectorAll('#items_timeline svg').length > 0)
        return;

    var width = $('.content').width();
    scaleX = d3.scaleTime([d3.utcDay.offset(new Date(), -7), d3.utcDay.offset(new Date(), 7)], [0, width - 2 * marginSide]);
    var axisX = d3.axisBottom(scaleX);

    svgElement = d3.select("#items_timeline")
        .append("svg")
        .attr("viewBox", `0 0 ${width} ${height}`);

    svgElement
        .append("g")
        .attr("transform", `translate(${marginSide},${height - marginBottom})`)
        .call(axisX);

    items_timeline.append(svg.node());
};

export function setTimelineEvents(events, noReset)
{
    if (noReset)
        svgElement.selectAll("circle").remove();

    svgElement.selectAll("circle")
        .data(events)
        .enter()
        .append("g")
            .attr("transform", function (e) { return `translate(${marginSide - 2 + scaleX(new Date(e.time))},${height - 50})`; })
        .append("circle")
            .attr("r", eventSize)
            .attr("fill", function (e) { return d3.color(e.color); })
            .attr("stroke", function (e) { return d3.color(e.color).darker(.5); })
            .attr("stroke-thickness", "0.5px")
            .attr("data-id", function (e) { return e.id; })

        .on('click', function () {
            var scrollElement = document.getElementById(this.dataset.id);
            scrollElement.classList.add('highlight');
            setTimeout(() => {
                scrollElement.classList.remove('highlight');
            }, 2500);

            window.scrollTo({
                behavior: 'smooth',
                top:
                    scrollElement.getBoundingClientRect().top -
                    document.body.getBoundingClientRect().top -
                    document.querySelectorAll('.top-row')[0].getBoundingClientRect().height,
            });

        })

        .on('mouseover', function (_, i) {
            highlighted = svgElement.selectAll(`[data-id="${i.id}"]`)
                .attr("stroke", "black")
                .attr("stroke-thickness", "1px");

            var newG = d3.select(this.parentNode)
                .append("g")
                .attr("transform", "translate(0, -25)")
                .attr("letter-spacing", "1px");

            newG.append("text")
                .attr("stroke", "black")
                .attr("transform", "translate(0, -10)")
                .attr("font-size", "12px")
                .attr("font-weight", "lighter")
                .attr("text-anchor", "middle")
                .text(i.bezeichnung);

            var timeFormatDate = d3.timeFormat("%a - %d.%m.%Y");
            var timeFormatDateWithTime = d3.timeFormat("%a - %d.%m.%Y - %H:%M");
            newG.append("text")
                .attr("stroke", "black")
                .attr("transform", "translate(0, 10)")
                .attr("font-size", "12px")
                .attr("font-weight", "lighter")
                .attr("text-anchor", "middle")
                .text(function () {
                    var itemDate = new Date(i.time);
                    if (i.timeIsSet)
                        return timeFormatDateWithTime(itemDate);
                    else
                        return timeFormatDate(itemDate);
                });

            d3.select(this)
                .transition()
                .duration('500')
                .attr('opacity', '.75')
                .duration('250')
                .attr('r', eventSize * 2);

            var highlightElement = document.getElementById(this.dataset.id);
            highlightElement.classList.add('highlight');
        })

        .on('mouseout', function () {
            highlighted
                .attr("stroke", function (e) { return d3.color(e.color).darker(.5); })
                .attr("stroke-thickness", "0.5px");

            highlighted = undefined;

            d3.select(this.parentNode).select("g").remove();
            d3.select(this)
                .transition()
                .duration('500')
                .attr('opacity', '1')
                .duration('250')
                .attr('r', eventSize);

            var scrollElement = document.getElementById(this.dataset.id);
            scrollElement.classList.remove('highlight');
        });

    $(d3.selectAll('.fa.fa-clock').nodes()).closest('tr')
        .on('mouseover', function () {
            highlighted = svgElement.selectAll(`[data-id="${this.id}"]`)
                .transition()
                .duration('500')
                .attr("stroke", "black")
                .attr("stroke-thickness", "1px")
                .attr('opacity', '.75')
                .duration('250')
                .attr('r', eventSize * 2);
        })
        .on('mouseout', function () {
            highlighted
                .transition()
                .duration('500')
                .attr("stroke", function (e) { return d3.color(e.color).darker(.5); })
                .attr("stroke-thickness", "0.5px")
                .attr('opacity', '1')
                .duration('250')
                .attr('r', eventSize);

            highlighted = undefined;
       });
};