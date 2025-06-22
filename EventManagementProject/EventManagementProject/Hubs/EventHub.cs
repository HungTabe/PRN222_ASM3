using EventManagementProject.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace EventManagementProject.Hubs
{
    public class EventHub : Hub
    {
        // Update attendee count
        public async Task SendAttendeeUpdate(int eventId, int attendeeCount)
        {
            await Clients.All.SendAsync("ReceiveAttendeeUpdate", eventId, attendeeCount);
        }

        // Update event information
        public async Task SendEventUpdate(int eventId, EventDetailViewModel eventData)
        {
            await Clients.All.SendAsync("ReceiveEventUpdate", eventId, eventData);
        }

        // Update total attendee number report information
        public async Task SendAttendeeReportUpdate(int numberAttendeeUpdate)
        {
            await Clients.All.SendAsync("ReceiveAttendeeNumberUpdate", numberAttendeeUpdate);
        }
    }
}
