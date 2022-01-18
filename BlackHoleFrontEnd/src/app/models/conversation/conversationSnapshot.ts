export class ConversationSnapshot {
    public conversationId: string;
    public name: string;
    public text: string;
    public lastMessageTime: Date;

    public constructor(convesationId: string, name: string, text: string, lastMessageTime: Date) {
        this.conversationId = convesationId;
        this.name = name;
        this.text = text;
        this.lastMessageTime = lastMessageTime;
    }
}