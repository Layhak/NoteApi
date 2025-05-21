import axios from 'axios'
import type { Note } from '@/types'

const apiClient = axios.create({
  baseURL: 'https://localhost:7081/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

// Add interceptor for auth token
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export default {
  // Notes
  getNotes() {
    return apiClient.get<Note[]>('/notes')
  },
  getNote(id: number) {
    return apiClient.get<Note>(`/notes/${id}`)
  },
  createNote(note: Omit<Note, 'id' | 'createdAt' | 'updatedAt' | 'userId'>) {
    return apiClient.post<Note>('/notes', note)
  },
  updateNote(id: number, note: Pick<Note, 'title' | 'content'>) {
    return apiClient.put<Note>(`/notes/${id}`, note)
  },
  deleteNote(id: number) {
    return apiClient.delete(`/notes/${id}`)
  },

  // Auth (optional)
  login(username: string, password: string) {
    return apiClient.post('/auth/login', { username, password })
  },
  register(username: string, password: string) {
    return apiClient.post('/auth/register', { username, password })
  },
}
