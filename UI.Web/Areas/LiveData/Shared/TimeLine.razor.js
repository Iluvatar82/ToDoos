import "/src/d3/d3.min.js";
import "/jquery/jquery.js";

let svgElement, scaleX, xAxis, highlighted;

var height = 105;
var marginBottom = 21;
var marginSide = 20;
var eventSize = 5;

export function InitializeTimeline()
{
    if (document.querySelectorAll('#items_timeline svg').length > 0)
        return;

    var width = $('.content').width();
    scaleX = d3.scaleTime([d3.utcDay.offset(new Date(), -2), d3.utcDay.offset(new Date(), 7)], [0, width - 2 * marginSide]);
    xAxis = d3.axisBottom(scaleX);

    svgElement = d3.select("#items_timeline")
        .append("svg")
        .attr("viewBox", `0 0 ${width} ${height}`);

    svgElement
        .append("g")
        .attr("class", "x")
        .attr("transform", `translate(${marginSide},${height - marginBottom})`)
        .call(xAxis);

    items_timeline.append(svgElement.node());
};

export function SetTimeRange(start, end)
{
    scaleX.domain([new Date(Date.parse(start)), new Date(Date.parse(end))]);

    svgElement.selectAll("g.x")
        .transition()
        .duration(500)
        .call(xAxis);
};

export function SetTimelineEvents(events)
{
    const trans = svgElement.transition().duration(250);

    svgElement.selectAll("svg g[data-item]")
        .data(events, function (e) { return Key(e); })
        .call(updateTrans => updateTrans.transition(trans)
            .attr("transform", function (e) { return `translate(${marginSide - 2 + scaleX(new Date(e.time))}, ${height - 50})`; }));

    svgElement.selectAll("svg g[data-item]")
        .data(events, function (e) { return Key(e); })
        .exit()
        .call(exitTrans => exitTrans.transition(trans)
            .attr("transform", function (e) { return `translate(${marginSide - 2 + scaleX(new Date(e.time))}, ${-2 * eventSize})`; })
            .attr("opacity", 0)
            .remove());

    svgElement.selectAll("svg g[data-item]")
        .data(events, function (e) { return Key(e); })
        .enter()
        .append("g")
            .attr("data-item", "")
            .attr("transform", function (e) { return `translate(${marginSide - 2 + scaleX(new Date(e.time))},${height + 2 * eventSize})`; })
            .attr("opacity", 0)
            .call(enterTrans => enterTrans.transition(trans)
                .attr("transform", function (e) { return `translate(${marginSide - 2 + scaleX(new Date(e.time))},${height - 50})`; })
                .attr("opacity", 1))
        .append("circle")
            .attr("r", eventSize)
            .attr("fill", function (e) { return d3.color(e.color); })
            .attr("stroke", function (e) { return d3.color(e.color).darker(.5); })
            .attr("stroke-thickness", "0.5px")
            .attr("data-id", function (e) { return e.id; })

        .on('click', function () {
            var scrollElement = document.getElementById(this.dataset.id);
            if (scrollElement === null) {
                var listId = d3.select(this).data()[0].listId;

                var listLink = document.querySelector(`.nav-item a[href*="${listId}"`)
                listLink.click();
                //window.location.href = `/list/${d3.select(this).data()[0].listId}`;
                return;
            }

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
            if (highlightElement != undefined)
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

            var highlightElement = document.getElementById(this.dataset.id);
            if (highlightElement != undefined)
                highlightElement.classList.remove('highlight');
        });

    $(d3.selectAll('.fa.fa-clock').nodes()).closest('tr')
        .on('mouseover', function () {
            svgElement.selectAll(`[data-id="${this.id}"]`)
                .transition()
                .duration('500')
                .attr("stroke", "black")
                .attr("stroke-thickness", "1px")
                .attr('opacity', '.75')
                .duration('250')
                .attr('r', eventSize * 2);
        })
        .on('mouseout', function () {
            svgElement.selectAll(`[data-id="${this.id}"]`)
                .transition()
                .duration('500')
                .attr("stroke", function (e) { return d3.color(e.color).darker(.5); })
                .attr("stroke-thickness", "0.5px")
                .attr('opacity', '1')
                .duration('250')
                .attr('r', eventSize);
       });
};

function Key(e) { return e.id + e.bezeichnung + e.time + e.color };