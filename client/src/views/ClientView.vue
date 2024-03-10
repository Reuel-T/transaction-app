<template>
  <AppPageWrapper>
    <div class="container">
      <h1 class="title">Client Transactions</h1>

      <div
        class="container"
        v-if="foundClient && !isClientError"
      >
        <!-- Client Info -->
        <ClientInfoCard
          :client="client"
          :is-loading="isClientLoading"
        />

        <!-- Create Transaction Form -->
        <CreateClientTransactionForm
          @post-success="
            (data: ClientTransactionDTO) => {
              refetchClient()
              console.log(data)
              if (transactions) {
                transactions.push(data)
              }
            }
          "
          :client-id="Number($route.params.id)"
        />

        <div class="container card">
          <ClientTransactions
            v-if="
              !isTransactionsError && !isTransactionsLoading && !transactions404 && transactions
            "
            :transactions="transactions"
          />
        </div>
      </div>
    </div>
  </AppPageWrapper>
</template>

<script setup lang="ts">
  import AppPageWrapper from '@/components/global/AppPageWrapper.vue'
  import ClientInfoCard from '@/components/page/client-transaction/ClientInfoCard.vue'
  import ClientTransactions from '@/components/page/client-transaction/ClientTransactions.vue'
  import CreateClientTransactionForm from '@/components/page/client-transaction/CreateClientTransactionForm.vue'
  import { useGetClient } from '@/shared/useGetClient'
  import { useGetClientTransactions } from '@/shared/useGetClientTransactions'
  import { useRoute } from 'vue-router'
  import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'

  const route = useRoute()
  const {
    client,
    foundClient,
    isError: isClientError,
    isLoading: isClientLoading,
    fetchClient: refetchClient
  } = useGetClient(Number(route.params.id))

  const {
    transactions,
    is404: transactions404,
    isLoading: isTransactionsLoading,
    isError: isTransactionsError
  } = useGetClientTransactions(Number(route.params.id))
</script>

<style lang="scss" scoped>
  .card {
    margin: 2rem 0;
    backdrop-filter: blur(1px);
    border-radius: 1rem;
    border: 1px solid rgb(48, 48, 48);
    box-shadow: 0.05em 0.05em 1em rgba(255, 255, 255, 0.1);
    padding: 1rem;
  }

  .container {
    height: 100%;
    display: flex;
    flex-direction: column;
    overflow-y: hidden;
  }
</style>
