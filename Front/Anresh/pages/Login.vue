<template>
    <section class="empty-section">
        <login-form class="form-wrap"/>
    </section>
</template>

<script>
import LoginForm from '../components/Auth/LoginForm.vue'

export default {
  components: { LoginForm },
  layout: 'empty'
}
</script>