const mesesAbreviados = [
    "Jan", "Fev", "Mar", "Abr", "Mai", "Jun",
    "Jul", "Ago", "Set", "Out", "Nov", "Dez"
];

function monthsBetweenDates(startDate, endDate) {
    const parseCustomDate = (dateString) => {
        const [year, month] = dateString.split('-').map(Number);
        return new Date(year, month - 1, 1);
    };

    const start = parseCustomDate(startDate);
    const end = parseCustomDate(endDate);
    const months = [];

    for (let currentDate = new Date(start); currentDate <= end; currentDate.setMonth(currentDate.getMonth() + 1)) {
        months.push(new Date(currentDate));
    }

    return months;
}

function populateChartData(values, meses) {
    return meses.map(mes => {
        const monthStr = `${mes.getMonth() + 1}/${mes.getFullYear()}`;
        const valueObj = values.find(item => item.month === monthStr);
        return valueObj ? parseFloat(valueObj.value) : 0;
    });
}

function createChart(ctx, type, labels, data, options) {
    return new Chart(ctx, {
        type,
        data: {
            labels,
            datasets: [data]
        },
        options
    });
}

function InitDelinquency(valuesDelinquency, startDate, endDate) {
    const meses = monthsBetweenDates(startDate, endDate);
    const valoresInadimplencia = populateChartData(valuesDelinquency, meses);
    const ctxInadimplencia = document.getElementById('graficoInadimplencia').getContext('2d');

    createChart(ctxInadimplencia, 'bar', meses.map(mes => mesesAbreviados[mes.getMonth()]), {
        label: '',
        data: valoresInadimplencia,
        borderColor: '#dc3545',
        backgroundColor: '#fb6340',
        borderWidth: 2,
        borderRadius: 5,
        borderSkipped: false,
        barPercentage: 0.4
    }, {
        plugins: {
            legend: {
                display: false
            },
            tooltip: {
                displayColors: false,
                callbacks: {
                    label: function (tooltipItem) {
                        return 'R$ ' + tooltipItem.raw.toFixed(2).replace('.', ',');
                    }
                }
            }
        },
        scales: {
            y: {
                grid: {
                    lineWidth: 1,
                    color: '#212529',
                },
                ticks: {
                    color: '#fff',
                    callback: function (value) {
                        return 'R$ ' + value.toLocaleString('pt-BR');
                    }
                }
            },
            x: {
                grid: {
                    display: false
                },
                ticks: {
                    color: '#fff',
                }
            }
        }
    });
}

function InitPastDue(valuesPastDue, startDate, endDate) {
    const meses = monthsBetweenDates(startDate, endDate);
    const valoresReceitaRecebida = populateChartData(valuesPastDue, meses);
    const ctxReceita = document.getElementById('graficoReceita').getContext('2d');

    createChart(ctxReceita, 'line', meses.map(mes => mesesAbreviados[mes.getMonth()]), {
        label: 'Performance',
        data: valoresReceitaRecebida,
        cubicInterpolationMode: 'monotone',
        fill: true,
        tension: 0.4,
        borderColor: '#84c0ec',
    }, {
        scales: {
            y: {
                grid: {
                    lineWidth: 1,
                    color: '#fb6340',
                    zeroLineColor: '#fb6340'
                },
                ticks: {
                    color: '#fff',
                    callback: function (value) {
                        return 'R$ ' + value.toLocaleString('pt-BR');
                    }
                }
            },
            x: {
                grid: {
                    display: false
                },
                ticks: {
                    color: '#fff'
                }
            }
        },
        plugins: {
            legend: {
                display: false
            },
            tooltip: {
                displayColors: false,
                callbacks: {
                    label: function (tooltipItem) {
                        return 'R$ ' + tooltipItem.raw.toFixed(2).replace('.', ',');
                    }
                }
            }
        }
    });
}
