export class BaseMessage {
    public conversationId: string;
    public messageId: string;
    public repliedMessage: BaseMessage;
    public text: string;
    public time: Date;
    public seen: boolean;
}