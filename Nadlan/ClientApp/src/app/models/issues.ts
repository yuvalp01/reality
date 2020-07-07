import { IApartment, IAppUser } from ".";

export interface IIssue {
  id: number;
  title: string;
  description: string;
  priority: number;
  dateOpen: Date;
  dateClose : Date;
  aprtment: IApartment;
  isDeleted: boolean;
  messages: IMessage[]
}
export interface IMessage {
  id: number;
  description: string;
  priority: number;
  userName: string;
  dateStemp: Date;
  issue : IIssue;
  isRead: boolean;
  isDeleted: boolean;
}


