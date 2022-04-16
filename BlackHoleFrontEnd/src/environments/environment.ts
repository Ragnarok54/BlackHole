// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  baseApiUrl: 'https://localhost:44340/api',
  baseHubUrl: 'https://localhost:44340',
};

export const LOGIN_URL = environment.baseApiUrl + '/user/login'; 
export const REGISTER_URL = environment.baseApiUrl + '/user/register';
export const CONVERSATIONS_URL = environment.baseApiUrl + "/conversations";
export const CONVERSATION_NAME_URL = environment.baseApiUrl + "/conversation/name"
export const CONVERSATION_MESSAGES_URL = environment.baseApiUrl + '/conversation';
export const MESSAGE_URL = environment.baseApiUrl + '/messages/send';
export const MESSAGE_HUB_URL = environment.baseHubUrl + '/Messages/Hub';
export const CONTACTS_URL = environment.baseApiUrl + '/Conversation/Contacts';
export const CONVERSATION_ADD_URL = environment.baseApiUrl + '/conversation/Add';
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
