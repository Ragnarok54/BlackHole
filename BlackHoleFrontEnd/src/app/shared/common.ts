import { Capacitor } from "@capacitor/core";
import { environment } from "src/environments/environment";

export class Common {
    public static PhoneNumberRegEx = "^07\\d{2}\\s?\\d{3}\\s?\\d{3}$";

    public static LOGIN_URL = environment.baseApiUrl + '/user/login'; 
    public static REGISTER_URL = environment.baseApiUrl + '/user/register';
    public static CONVERSATIONS_URL = environment.baseApiUrl + "/conversations";
    public static CONVERSATION_DETAILS_URL = environment.baseApiUrl + "/conversation/details"
    public static CONVERSATION_MESSAGES_URL = environment.baseApiUrl + '/conversation';
    public static MESSAGE_URL = environment.baseApiUrl + '/messages/send';
    public static MESSAGE_HUB_URL = environment.baseHubUrl + '/Messages/Hub';
    public static CONTACTS_URL = environment.baseApiUrl + '/Conversation/Contacts?query=';
    public static USER_URL = environment.baseApiUrl + '/User';
    public static CONVERSATION_ADD_URL = environment.baseApiUrl + '/conversation/Add';
    public static CONVERSATION_SEEN_URL = environment.baseApiUrl + '/conversation/Seen';

    public static IS_MOBILE = Capacitor.getPlatform() == 'ios';
}