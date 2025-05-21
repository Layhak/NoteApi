<template>
  <div class="p-4 max-w-4xl mx-auto">
    <div v-if="loading" class="text-center py-8">
      <p>Loading note...</p>
    </div>

    <div v-else-if="error" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
      {{ error }}
    </div>

    <div v-else-if="!note" class="text-center py-8">
      <p>Note not found</p>
      <button @click="router.push('/notes')" class="mt-4 px-4 py-2 bg-blue-500 text-white rounded">
        Back to Notes
      </button>
    </div>

    <div v-else>
      <div class="mb-4 flex justify-between items-center">
        <button @click="router.push('/notes')" class="text-blue-500 hover:text-blue-700">
          ← Back to Notes
        </button>

        <div class="flex gap-2">
          <button @click="editMode = true" class="px-4 py-2 bg-green-500 text-white rounded">
            Edit
          </button>
          <button @click="confirmDelete = true" class="px-4 py-2 bg-red-500 text-white rounded">
            Delete
          </button>
        </div>
      </div>

      <div v-if="!editMode" class="bg-white rounded-lg shadow-md p-6">
        <h1 class="text-2xl font-bold mb-2">{{ note.title }}</h1>
        <div class="flex gap-4 text-sm text-gray-500 mb-6">
          <p>Created: {{ formatDate(note.createdAt) }}</p>
          <p>Updated: {{ formatDate(note.updatedAt) }}</p>
        </div>
        <div class="prose max-w-none">
          <p v-if="note.content" class="whitespace-pre-wrap">{{ note.content }}</p>
          <p v-else class="text-gray-500 italic">No content</p>
        </div>
      </div>

      <div v-else class="bg-white rounded-lg shadow-md p-6">
        <NoteForm :note="note" @save="saveNote" @cancel="editMode = false" />
      </div>
    </div>

    <!-- Delete Confirmation Modal -->
    <div
      v-if="confirmDelete"
      class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4"
    >
      <div class="bg-white rounded-lg p-6 w-full max-w-md">
        <h2 class="text-xl font-bold mb-4">Delete Note</h2>
        <p>Are you sure you want to delete "{{ note?.title }}"?</p>
        <div class="flex justify-end gap-2 mt-4">
          <button @click="confirmDelete = false" class="px-4 py-2 border rounded">Cancel</button>
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
import { useRoute, useRouter } from 'vue-router'
import { useNotesStore } from '@/stores/notes'
import NoteForm from '@/components/NoteForm.vue'

const route = useRoute()
const router = useRouter()
const notesStore = useNotesStore()

const editMode = ref(false)
const confirmDelete = ref(false)

const loading = computed(() => notesStore.loading)
const error = computed(() => notesStore.error)
const note = computed(() => notesStore.currentNote)

onMounted(async () => {
  const noteId = Number(route.params.id)
  if (!isNaN(noteId)) {
    await notesStore.fetchNote(noteId)
  }
})

function formatDate(dateString: string): string {
  const date = new Date(dateString)
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  }).format(date)
}

async function saveNote(data: { title: string; content: string; id?: number }) {
  if (note.value) {
    try {
      await notesStore.updateNote(note.value.id, data)
      editMode.value = false
    } catch (error) {
      console.error('Failed to update note:', error)
    }
  }
}

async function deleteNote() {
  if (note.value) {
    try {
      await notesStore.deleteNote(note.value.id)
      router.push('/notes')
    } catch (error) {
      console.error('Failed to delete note:', error)
    }
  }
}
</script>
