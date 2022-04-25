import { BaseMessage } from "../message/baseMessage";

export class ConversationSnapshot {
    public conversationId: string;
    public name: string;
    public lastMessage: BaseMessage;
}