import { IApartment, IAppUser } from ".";

export interface IIssue {
  id: number;
  title: string;
  description: string;
  priority: number;
  dateOpen: Date;
  dateClose: Date;
  aprtment: IApartment;
  isNew: boolean;
  createdBy: string;
  isDeleted: boolean;
  messages: IMessage[];
  stakeholderId: number;
}
export interface IMessage {
  id: number;
  content: string;
  dateStemp: Date;
  userName: string;
  parentId: number;
  tableName: string;
  isRead: boolean;
  isDeleted: boolean;
}


