import { defineStore } from 'pinia'
import type { Note, NoteFilter } from '@/types'
import api from '@/services/api'

export const useNotesStore = defineStore('notes', {
  state: () => ({
    notes: [] as Note[],
    currentNote: null as Note | null,
    loading: false,
    error: null as string | null,
    filter: {
      searchTerm: '',
      sortBy: 'updatedAt' as const,
      sortDirection: 'desc' as const,
    },
  }),

  getters: {
    filteredNotes: (state) => {
      let result = [...state.notes]

      // Apply search filter
      if (state.filter.searchTerm) {
        const term = state.filter.searchTerm.toLowerCase()
        result = result.filter(
          (note) =>
            note.title.toLowerCase().includes(term) || note.content.toLowerCase().includes(term),
        )
      }

      // Apply sorting
      result.sort((a, b) => {
        const field = state.filter.sortBy
        const direction = state.filter.sortDirection === 'asc' ? 1 : -1

        if (field === 'title') {
          return direction * a.title.localeCompare(b.title)
        } else {
          const dateA = new Date(a[field]).getTime()
          const dateB = new Date(b[field]).getTime()
          return direction * (dateA - dateB)
        }
      })

      return result
    },
  },

  actions: {
    async fetchNotes() {
      this.loading = true
      this.error = null
      try {
        const response = await api.getNotes()
        this.notes = response.data
      } catch (err) {
        this.error = 'Failed to fetch notes'
        console.error(err)
      } finally {
        this.loading = false
      }
    },

    async fetchNote(id: number) {
      this.loading = true
      this.error = null
      try {
        const response = await api.getNote(id)
        this.currentNote = response.data
      } catch (err) {
        this.error = 'Failed to fetch note'
        console.error(err)
      } finally {
        this.loading = false
      }
    },

    async createNote(note: { title: string; content: string }) {
      this.loading = true
      this.error = null
      try {
        const response = await api.createNote(note)
        this.notes.push(response.data)
        return response.data
      } catch (err) {
        this.error = 'Failed to create note'
        console.error(err)
        throw err
      } finally {
        this.loading = false
      }
    },

    async updateNote(id: number, note: { title: string; content: string }) {
      this.loading = true
      this.error = null
      try {
        const response = await api.updateNote(id, note)
        const index = this.notes.findIndex((n) => n.id === id)
        if (index !== -1) {
          this.notes[index] = response.data
        }
        if (this.currentNote?.id === id) {
          this.currentNote = response.data
        }
        return response.data
      } catch (err) {
        this.error = 'Failed to update note'
        console.error(err)
        throw err
      } finally {
        this.loading = false
      }
    },

    async deleteNote(id: number) {
      this.loading = true
      this.error = null
      try {
        await api.deleteNote(id)
        this.notes = this.notes.filter((note) => note.id !== id)
        if (this.currentNote?.id === id) {
          this.currentNote = null
        }
      } catch (err) {
        this.error = 'Failed to delete note'
        console.error(err)
        throw err
      } finally {
        this.loading = false
      }
    },

    setFilter(filter: Partial<NoteFilter>) {
      this.filter = { ...this.filter, ...filter }
    },
  },
})
