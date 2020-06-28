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
}
export interface IMessage {
  id: number;
  description: string;
  priority: number;
  user: IAppUser;
  issue : IIssue;
  isDeleted: boolean;
}


