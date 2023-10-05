import { StoryPost } from "./post";

export interface Comment {
  commentId: number;
  commentContent: string | null;
  id: number | null;
  storyPostNav: StoryPost | null;
}
