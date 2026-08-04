using Coravel.Invocable;
using Core.Domain;
using Core.Enumeration;
using Core.Response;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace Api.Provider
{
    public class AppointmentCron : IInvocable
    {
        private readonly WhatsAppProvider _whatsappprvoider;

        public AppointmentCron(WhatsAppProvider whatsAppProvider)
        {
            _whatsappprvoider = whatsAppProvider;
        }

        public async Task Invoke()
        {
            //    Console.WriteLine("Appointment notification started at " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"));

            //    string json = await _appointmentRepository.GetTodayAppointments();
            //    if (string.IsNullOrEmpty(json)) return;

            //    var appointments = JsonSerializer.Deserialize<List<AppointmentReminderResponse>>(json);
            //    if (appointments == null || !appointments.Any()) return;

            //    var currentDateTime = DateTime.Now;

            //    foreach (var appointment in appointments)
            //    {
            //        var timeDifference = appointment.Start_time - currentDateTime;
            //        // Send reminder of the appointment before 1 hour  (within a 2-minute window)
            //        if (timeDifference.TotalMinutes >= 59 && timeDifference.TotalMinutes <= 62)
            //        {
            //            string patient_name = appointment.Patient_name;
            //            string doctor_name = appointment.Doctor_name;
            //            string date = appointment.Start_time.ToString("dd-MMM-yyyy");
            //            string time = appointment.Start_time.ToString("hh:mm tt");

            //            string message = string.Format(MessageProvider.APPOINTMENT_REMINDER, patient_name, appointment.Company_name, date, time, appointment.Company_contact_no, appointment.Company_name);
            //            string phoneNumber = appointment.Patient_number?.Trim() ?? string.Empty;

            //            if (!string.IsNullOrEmpty(phoneNumber))
            //            {
            //                if (!phoneNumber.StartsWith("91"))
            //                {
            //                    phoneNumber = "91" + phoneNumber;
            //                }
            //            }
            //            if (!string.IsNullOrEmpty(phoneNumber))
            //            {
            //                bool isSent = await _whatsappprvoider.Send(phoneNumber, message);

            //                MessageLog msgLog = new()
            //                {
            //                    Message_type = (short)MessageType.WhatsApp,
            //                    Company_id = appointment.Company_id,
            //                    Patient_id = appointment.Patient_id,
            //                    Send_to = phoneNumber,
            //                    Subject = "Appointment Reminder",
            //                    Created_by = "System",
            //                    Created_date = DateTime.Now,
            //                    Is_success = isSent
            //                };
            //                await _messageLogRepository.Insert(msgLog);

            //            }
            //        }
            //    }
        }
    }
}