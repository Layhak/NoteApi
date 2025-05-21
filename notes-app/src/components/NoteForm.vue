<template>
  <form @submit.prevent="handleSubmit">
    <div class="mb-4">
      <label for="title" class="block text-sm font-medium text-gray-700 mb-1">Title</label>
      <input
        id="title"
        v-model="formData.title"
        type="text"
        required
        class="w-full px-3 py-2 border rounded-md"
        placeholder="Note title"
      />
    </div>

    <div class="mb-4">
      <label for="content" class="block text-sm font-medium text-gray-700 mb-1">Content</label>
      <textarea
        id="content"
        v-model="formData.content"
        rows="6"
        class="w-full px-3 py-2 border rounded-md"
        placeholder="Note content"
      ></textarea>
    </div>

    <div class="flex justify-end gap-2">
      <button type="button" @click="$emit('cancel')" class="px-4 py-2 border rounded">
        Cancel
      </button>
      <button
        type="submit"
        class="px-4 py-2 bg-blue-500 text-white rounded"
        :disabled="!formData.title.trim()"
      >
        Save
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { Note } from '@/types'

const props = defineProps<{
  note?: Note | null
}>()

const emit = defineEmits<{
  (e: 'save', data: { title: string; content: string; id?: number }): void
  (e: 'cancel'): void
}>()

const formData = ref({
  title: '',
  content: '',
})

onMounted(() => {
  if (props.note) {
    formData.value.title = props.note.title
    formData.value.content = props.note.content
  }
})

function handleSubmit() {
  if (!formData.value.title.trim()) return

  const data = {
    title: formData.value.title.trim(),
    content: formData.value.content.trim(),
  }

  if (props.note) {
    emit('save', { ...data, id: props.note.id })
  } else {
    emit('save', data)
  }
}
</script>
