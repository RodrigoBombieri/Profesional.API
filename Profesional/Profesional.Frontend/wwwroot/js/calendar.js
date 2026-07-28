window.inicializarCalendario = function (eventos) {
    var calendarEl = document.getElementById('calendar');
    if (calendarEl) {
        var calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridMonth',
            locale: 'es',
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
            },
            events: eventos,
            eventClick: function (info) {
                Swal.fire({
                    title: info.event.title,
                    html: `
                        <p><strong>Fecha:</strong> ${info.event.start.toLocaleString()}</p>
                        <p><strong>Duración:</strong> ${info.event.extendedProps.duracion || '30'} min</p>
                        <p><strong>Observaciones:</strong> ${info.event.extendedProps.observaciones || 'Sin observaciones'}</p>
                        <p><strong>Estado:</strong> ${info.event.extendedProps.completada ? '✅ Completada' : '⏳ Pendiente'}</p>
                    `,
                    icon: 'info',
                    confirmButtonText: 'Ver Paciente',
                    showCancelButton: true,
                    cancelButtonText: 'Cerrar'
                }).then((result) => {
                    if (result.isConfirmed && info.event.extendedProps.pacienteId) {
                        window.location.href = `/pacientes/${info.event.extendedProps.pacienteId}`;
                    }
                });
            },
            eventDidMount: function (info) {
                // Tooltip personalizado
                info.el.title = `${info.event.title}\n${info.event.start.toLocaleString()}`;
            }
        });
        calendar.render();
    }
};