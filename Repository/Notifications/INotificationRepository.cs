using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
  public interface INotificationRepository
  {

    public Task<ResponseStatus> AddContact(ContactListDto contacts);

    public Task<IEnumerable<Contacts>> GetContacts();


    public Task<ResponseStatus> AddNotification(NotificationListDTO notificationList);


    // public Task<ResponseStatus> Sendsms(SmsRequestListDTO SmsPayload);

    public Task<string> GetAccessToken();

    public Task<IEnumerable<MessageTemplates>> GetSmsTemplates();

    public Task<ResponseStatus> AddSmsTemplate(MessageTemplateListDTO TemplatePayload);

    public Task<IEnumerable<Notification>> PendingNotifications();

  }

}