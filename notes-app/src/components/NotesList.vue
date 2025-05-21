<template>
  <div class="p-4">
    <div class="mb-6 flex flex-col md:flex-row md:items-center justify-between gap-4">
      <h1 class="text-2xl font-bold">My Notes</h1>

      <div class="flex flex-col sm:flex-row gap-4">
        <SearchBar v-model="searchTerm" @search="handleSearch" placeholder="Search notes..." />

        <div class="flex items-center gap-2">
          <select
            v-model="sortBy"
            class="border rounded px-2 py-1 text-sm"
            @change="handleSortChange"
          >
            <option value="updatedAt">Last Updated</option>
            <option value="createdAt">Created Date</option>
            <option value="title">Title</option>
          </select>

          <button @click="toggleSortDirection" class="p-1 rounded border">
            <span v-if="sortDirection === 'asc'">↑</span>
            <span v-else>↓</span>
          </button>
        </div>
      </div>
    </div>

    <div class="mb-4">
      <button
        @click="showCreateForm = true"
        class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded"
      >
        Create New Note
      </button>
    </div>

    <div v-if="loading" class="text-center py-8">
      <p>Loading notes...</p>
    </div>

    <div v-else-if="error" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
      {{ error }}
    </div>

    <div v-else-if="filteredNotes.length === 0" class="text-center py-8">
      <p class="text-gray-500">No notes found. Create your first note!</p>
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      <NoteItem
        v-for="note in filteredNotes"
        :key="note.id"
        :note="note"
        @view="viewNote"
        @edit="editNote"
        @delete="confirmDelete"
      />
    </div>

    <!-- Create/Edit Modal -->
    <div
      v-if="showCreateForm || editingNote"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4"
    >
      <div class="bg-white rounded-lg p-6 w-full max-w-md">
        <h2 class="text-xl font-bold mb-4">
          {{ editingNote ? 'Edit Note' : 'Create New Note' }}
        </h2>

        <NoteForm :note="editingNote" @save="saveNote" @cancel="cancelForm" />
      </div>
    </div>

    <!-- Delete Confirmation Modal -->
    <div
      v-if="noteToDelete"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4"
    >
      <div class="bg-white rounded-lg p-6 w-full max-w-md">
        <h2 class="text-xl font-bold mb-4">Delete Note</h2>
        <p>Are you sure you want to delete "{{ noteToDelete.title }}"?</p>
        <div class="flex justify-end gap-2 mt-4">
          <button @click="noteToDelete = null" class="px-4 py-2 border rounded">Cancel</button>
          <button @click="deleteNote" class="px-4 py-2 bg-red-500 text-white rounded">
            Delete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useNotesStore } from '@/stores/notes'
import type { Note } from '@/types'
import NoteItem from '../components/NoteItem.vue'
import NoteForm from '../components/NoteForm.vue'
import SearchBar from '../components/SearchBar.vue'

const router = useRouter()
const notesStore = useNotesStore()

const showCreateForm = ref(false)
const editingNote = ref<Note | null>(null)
const noteToDelete = ref<Note | null>(null)

const searchTerm = ref('')
const sortBy = ref<'title' | 'createdAt' | 'updatedAt'>('updatedAt')
const sortDirection = ref<'asc' | 'desc'>('desc')

const loading = computed(() => notesStore.loading)
const error = computed(() => notesStore.error)
const filteredNotes = computed(() => notesStore.filteredNotes)

onMounted(() => {
  notesStore.fetchNotes()
})

function handleSearch(term: string) {
  searchTerm.value = term
  notesStore.setFilter({ searchTerm: term })
}

function handleSortChange() {
  notesStore.setFilter({ sortBy: sortBy.value })
}

function toggleSortDirection() {
  sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
  notesStore.setFilter({ sortDirection: sortDirection.value })
}

function viewNote(note: Note) {
  router.push(`/notes/${note.id}`)
}

function editNote(note: Note) {
  editingNote.value = { ...note }
}

function confirmDelete(note: Note) {
  noteToDelete.value = note
}

async function deleteNote() {
  if (noteToDelete.value) {
    try {
      await notesStore.deleteNote(noteToDelete.value.id)
      noteToDelete.value = null
    } catch (error) {
      console.error('Failed to delete note:', error)
    }
  }
}

async function saveNote(note: { title: string; content: string; id?: number }) {
  try {
    if (editingNote.value) {
      await notesStore.updateNote(editingNote.value.id, note)
      editingNote.value = null
    } else {
      await notesStore.createNote(note)
      showCreateForm.value = false
    }
  } catch (error) {
    console.error('Failed to save note:', error)
  }
}

function cancelForm() {
  showCreateForm.value = false
  editingNote.value = null
}
</script>
