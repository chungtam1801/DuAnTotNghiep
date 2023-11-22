﻿var ChartJS = {
    BarChart: function (idChartCanvas, labelsArray, datasArray, label) {
        new Chart($("#" + idChartCanvas), {
            type: 'bar',
            data: {
                labels: labelsArray,
                datasets: [{
                    backgroundColor: ["red", "green", "blue", "orange", "brown"],
                    data:datasArray
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display:false
                    },
                    title: {
                        display: true,
                        text: label,
                        font: {
                            size: 24
                        }
                    }
                },
                
            }
        });
    },
    LineChart: function (idChartCanvas, labelsArray, datasArray,label) {
        new Chart($("#" + idChartCanvas), {
            type: 'line',
            data: {
                labels: labelsArray,
                datasets: [{
                    backgroundColor: "rgba(0,0,255,1.0)",
                    borderColor: "rgba(0,0,255,0.1)",
                    data: datasArray
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
                    },
                    title: {
                        display: true,
                        text: label,
                        font: {
                            size: 16
                        }
                    }
                },
                scales: {
                    yAxes: [{ ticks: { min: 6, max: 16 } }],
                }
            }
        });
    },
    PieChart: function (idChartCanvas, labelsArray, datasArray, label) {
        new Chart($("#" + idChartCanvas), {
            type: 'pie',
            data: {
                labels: labelsArray,
                datasets: [{
                    backgroundColor: ["red", "green"],
                    data: datasArray
                }]
            },
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: label,
                        font: {
                            size: 10
                        }
                    }
                }
            }
        });
    }
}
