<template>
  <section>
    <div class="skill-wrap">
        <label style="color: #fff">Name</label>
      <b-form @submit.prevent="onSubmit" class="d-flex mb-5">
          
        <b-form-input
        class="mr-2"
          v-model="$v.form.name.$model"
          :state="validateState('name')"
          aria-describedby="skill-input-name-live-feedback"
        ></b-form-input>

        <b-form-invalid-feedback id="skill-input-name-live-feedback">
          {{
            $v.form.name.required
              ? "Please enter no more than 20 characters."
              : "This field is required."
          }}
        </b-form-invalid-feedback>
        <b-button type="submit" variant="primary" class="btn custom-btn mr-2" v-bind:disabled="$v.form.$invalid">
          {{nameButton}}
        </b-button>
        <b-button @click="resetForm()" variant="danger" class="btn custom-btn mr-2">
          Reset
        </b-button>
      </b-form>

      <ul v-if="isBusy" class="skills-list">
        <li @click="editSkill(skill)" v-for="skill in skills" :key="skill.id">
          {{ skill.name }}
          <span v-show="skill.id == form.id" class="selected"></span>
          <button @click="deleteSkill(skill.id)" class="delete-btn">
              <fa class="delete-btn-icon" icon="times"/>
          </button>
        </li>
      </ul>
      <div v-else class="text-center text-danger my-2">
        <b-spinner class="align-middle"></b-spinner>
        <strong>Loading...</strong>
      </div>

    </div>
  </section>
</template>

<script>
import { validationMixin } from "vuelidate";
import { required, maxLength } from "vuelidate/lib/validators";

export default {
  mixins: [validationMixin],
  data() {
    return {
      isBusy: false,
      skills: [],
      nameButton: 'Create',
      selectedSkill: null,
      form: {
        id: null,
        name: null,
      },
    };
  },

  validations: {
    form: {
      name: {
        required,
        maxLength: maxLength(20),
      },
    },
  },

  async mounted() {
    const skillUri = "/api/skill";
    let { data } = await this.$axios.get(skillUri);
    this.skills = data;
    this.isBusy = true;
  },

  methods: {
    validateState(name) {
      const { $dirty, $error } = this.$v.form[name];
      return $dirty ? !$error : null;
    },
    resetForm() {
      this.nameButton = 'Create';
      this.selectedSkill = null;
      this.form = {
        id: null,
        name: null,
      };

      this.$nextTick(() => {
        this.$v.$reset();
      });
    },
    editSkill(skill) {
      this.selectedSkill = skill;
      this.form.id = skill.id;
      this.form.name = skill.name;
      this.nameButton = 'Update';
    },

    async deleteSkill(id){
        
        await this.$axios
          .delete('/api/skill/' + id)
          .then((d) => {
           this.$notifyWarn('DELETED', this.selectedSkill.name);
           this.skills = this.skills.filter((s) => s.id !== id);
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
          this.resetForm();
    },

    async onSubmit() {
      if (this.form.id === null) {
        await this.$axios
          .post("/api/skill/", { name: this.form.name })
          .then((d) => {
            this.$notifyInfo("ADDED SKILL", this.form.name);
            this.skills.push(d.data.response);
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
      } else {
        await this.$axios
          .put("/api/skill/", this.form)
          .then((d) => {
            const index = this.skills.indexOf(this.selectedSkill);
            this.skills.splice(index, 1, this.form);
            this.$notifyInfo("UPDATED SKILL", this.form.name);
          })
          .catch((error) => {
            this.$notifyError("ERROR", `${error}`);
          });
      }
      this.resetForm();
    },
  },
};
</script>

<style lang="scss" scoped>

.skill-wrap {
  background-color: #404958;
  max-height: 60vh;
  border-radius: 7px;
  padding: .5rem;
  form{
      position: relative;
     #skill-input-name-live-feedback {
         position: absolute;
         bottom: -30px;
         background-color: #ee2b45;
         color: #fff;
         padding: .1rem .9rem;
         border-radius: 7px;
     } 
  }
  .skills-list {
    display: flex;
    justify-content: start;
    flex-wrap: wrap;
    padding: 0;
    max-height: 36vh;
    overflow-y: scroll;

    li {
      position: relative;
      list-style-type: none;
      margin: 0.5rem;
      background-color: #fff    ;
      padding: 0.5rem 1rem;
      border-radius: 20px;
      cursor: pointer;
      transition: 0.2s;
      .delete-btn {
            width: 30px;
            height: 30px;
            border-radius: 50%;
            background-color: #ee2b45;
            position: absolute;
            right: -1rem;
            bottom: -.5rem;
            transform: scale(0);
            transition: .2s;
            .delete-btn-icon {
                color: #fff;
            }
            &:hover .delete-btn-icon {
                color: #e7e7e7;
            }
        }
      .selected {
        position: absolute;
        width: 10px;
        height: 10px;
        border-radius: 5px;
        background-color: #28a745;
        right: -4px;
        top: -4px;
        transition: 0.2s;
        
      }
      &:hover {
        background-color: #c8c7c7;
        .delete-btn { 
            transform: scale(1);
        }
      }
    }
  }
}

@media (max-width: 500px) {
  .skill-wrap { 
    form {
        flex-wrap: wrap;
        input{
            margin-bottom: .5rem;
        }
    }
    }
}
</style>