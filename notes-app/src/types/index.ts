// src/types/index.ts
export type Note = {
  id: number
  title: string
  content: string
  createdAt: string
  updatedAt: string
  userId: number
}

export type User = {
  id: number
  username: string
  token?: string
}

export type NoteFilter = {
  searchTerm: string
  sortBy: 'title' | 'createdAt' | 'updatedAt'
  sortDirection: 'asc' | 'desc'
}
