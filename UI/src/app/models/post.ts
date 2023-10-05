
import { Hcuser } from "./hcuser";
import { Comment } from "./comment";

export interface StoryPost {
  pTitle: string;
  pDescription: string;
  createdOn?: string;
  like?: number;
  hCUserId?: string;
  hCUserNav?: Hcuser;
  comments?: Comment[];
}
