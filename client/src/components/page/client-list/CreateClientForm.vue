<template>
  <div class="card">
    <h2>Create Client</h2>
    <form @submit.prevent="onSubmit">
      <!-- Name -->
      <div class="form-group">
        <label for="name">Name</label>
        <input
          id="name"
          type="text"
          v-model.trim="formData.name"
          placeholder="Client Name"
          :disabled="isPosting"
          required
        />
      </div>

      <!-- Surname -->
      <div class="form-group">
        <label for="surname">Name</label>
        <input
          id="surname"
          type="text"
          v-model.trim="formData.surname"
          placeholder="Client Surname"
          :disabled="isPosting"
          required
        />
      </div>

      <!-- ClientBalance -->
      <div class="form-group">
        <label for="clientBalance">Opening Balance</label>
        <input
          id="clientBalance"
          type="number"
          v-model.number="formData.clientBalance"
          placeholder="R"
          :disabled="isPosting"
          required
        />
      </div>

      <!-- Submit Button -->
      <div class="form-group">
        <label
          for="submit"
          style="color: transparent"
          >Submit</label
        >
        <button
          id="submit"
          class="btn pill"
          type="submit"
          :disabled="isPosting"
        >
          Create
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
  import type { CreateClientDTO } from '@/models/CreateClientDTO'
  import { usePostClient } from '@/shared/usePostClient'
  import { reactive } from 'vue'

  const formData = reactive<CreateClientDTO>({
    name: '',
    surname: '',
    clientBalance: 0
  })

  //emit event, so the parent knows that this has been updated
  const emit = defineEmits(['client-posted'])

  const { isPosting, postData } = usePostClient({
    //emit the newly created client and reset the form
    onSuccess: (data) => {
      emit('client-posted', { ...data })
      formData.name = ''
      formData.surname = ''
      formData.clientBalance = 0
    }
  })

  function onSubmit() {
    postData(formData)
  }
</script>

<style lang="scss" scoped>
  form {
    display: flex;
    widows: 100%;

    .form-group {
      padding: 0.5rem;
      display: flex;
      flex-direction: column;
      flex-grow: 1;
    }
  }
</style>
