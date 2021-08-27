<template>
    <div id="creator">
            <div class="jumbotron card" style="float: left; margin-left: 2rem;">
                <h5 v-if="gender.male">Männlich</h5>
                <h5 v-else>Weiblich</h5>
                <ul class="parts">
                    <li v-for="(part, index) in parts" v-bind:key="index" @click="setActive($event, index)">
                        <i class="fas fa-caret-right"></i> {{ part.name }}
                    </li>
                </ul>
                <h6 class="text-align center font-bold" style="line-height: 1.5rem;">Kosten: <span class="color-text green" v-text="calculateTotal"></span> $</h6>
                <button class="btn secondary" style="margin-bottom: 0.3rem;" @click="changeGender">Geschlecht wechseln</button><br>
                <button class="btn positive" @click="finish">Fertig</button>
               
             </div>
             <div class="rotate">
                <i @click="rotate(-30)" class="fas fa-arrow-circle-left"></i>
                <i @click="rotate(30)" class="fas fa-arrow-circle-right"></i>
             </div>
            <div class="jumbotron card" style="float: right; margin-right: 2rem;">
                <h5 v-text="parts[currentPartIndex].name"></h5>
                <ul class="settings">
                    <li style="line-height: 3rem;" v-for="setting in getValidSettings" v-bind:key="setting.name">
                        <label style="margin-right: 2rem;" v-text="setting.name"></label>
                        <div style="float: right;">
                            <label style="margin-right: 1rem;" v-text="setting.value"></label>
                            <input v-model.number="setting.value" type="range" :min="setting.min" :max="setting.max" :step="setting.step"/>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
</template>

<script>
import Windows from "../windows"

export default Windows.register({
        name: "CharacterCreator",
        props: {
            data: String
        },
        data() {
            let obj = JSON.parse(this.data)
            return {
                currentPartIndex: 0,
                char: obj.customization,
                level: obj.level,
                gender: {
                    male: true,
                    default: true,
                    price: 500
                },
                parts: [
                {
                        name: "Kopfform",
                        method: "setHeadBlendData",
                        settings: [
                            {name: "Weibliche Gesichtsform", price: 100, value: 0, min: 0, max: 45, step: 1, default: 0},
                            {name: "Männliche Gesichtsform", price: 100, value: 0, min: 0, max: 45, step: 1, default: 0},
                            {value: 0},
                            {name: "Hautfarbe", price: 100, value: 0, min: 0, max: 45, step: 1, default: 0},
                            {name: "Hautfarbe", price: 100, value: 0, min: 0, max: 45, step: 1, default: 0},
                            {value: 0},
                            {name: "Formanteil weiblich/männlich", price: 100, value: 0, min: 0, max: 1, step: 0.1, default: 0},
                            {name: "Hautfarbenanteil weiblich/männlich", price: 100, value: 0, min: 0, max: 1, step: 0.1, default: 0},
                            {value: 0},
                            {value: false}
                        ]
                    },
                    {
                        name: "Gesicht",
                        method: "setFaceFeature",
                        settings: [
                            {name: "Nasenbreite", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Nasenhöhe", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Nasenlänge", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Nasenrücken", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Nasenspitze", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Nasenverkrümmung", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Augenbrauenhöhe", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Augenbrauenbreite", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Wangenknochenhöhe", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Wangenknochenbreite", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Wangenbreite", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Augen", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Lippen", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Kieferbreite", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Kieferhöhe", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Kinnlänge", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Kinnposition", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Kinnbreite", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Kinnform", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0},
                            {name: "Nackenbreite", price: 100, value: 0, min:-1, max: 1, step: 0.1, default: 0}
                        ]
                    },
                    {
                        name: "Frisur",
                        method: "setComponentVariation",
                        settings: [
                            {value: 2},
                            {name: "Frisur", price: 100, value: 0, min: 0, max: 20, step: 1, default: 0},
                            {value: 0},
                            {value: 0}
                        ]
                    },
                    {
                        name: "Haarfarbe",
                        method: "setHairColor",
                        settings: [
                            {name: "Farbe", price: 100, value: 0, min: 0, max: 50, step: 1, default: 0},
                            {name: "Stränenfarbe", price: 100, value: 0, min: 0, max: 50, step: 1, default: 0}
                        ]
                    },
                    {
                        name: "Augenfarbe",
                        method: "setEyeColor",
                        settings: [
                            {name: "Farbe", price: 100, value: 0, min: 0, max: 31, step: 1, default: 0}
                        ]
                    },
                    {
                        name: "Augenbrauen",
                        method: "setHeadOverlay",
                        settings: [
                            {value: 2},
                            {name: "Art ", price: 100, value: 0, min: 0, max: 33, step: 1, default: 0},
                            {name: "Deckkraft ", price: 100, value: 0, min: 0, max: 1, step: 0.1, default: 0},
                            {name: "Farbe ", price: 100, value: 0, min: 0, max: 50, step: 1, default: 0},
                            {value: 0}
                        ]
                    },
                    {
                        name: "Alter",
                        method: "setHeadOverlay",
                        settings: [
                            {value: 3},
                            {name: "Art ", price: 100, value: 0, min: 0, max: 14, step: 1, default: 0},
                            {name: "Deckkraft ", price: 100, value: 0, min: 0, max: 1, step: 0.1, default: 0},
                            {value: 0},
                            {value: 0}
                        ]
                    },
                    {
                        name: "Makeup",
                        method: "setHeadOverlay",
                        settings: [
                            {value: 4},
                            {name: "Art ", price: 100, value: 0, min: 0, max: 74, step: 1, default: 0},
                            {name: "Deckkraft ", price: 100, value: 0, min: 0, max: 1, step: 0.1, default: 0},
                            {value: 0},
                            {value: 0}
                        ]
                    },
                    {
                        name: "Blush",
                        method: "setHeadOverlay",
                        settings: [
                            {value: 5},
                            {name: "Art ", price: 100, value: 0, min: 0, max: 6, step: 1, default: 0},
                            {name: "Deckkraft ", price: 100, value: 0, min: 0, max: 1, step: 0.1, default: 0},
                            {name: "Farbe ", price: 100, value: 0, min: 0, max: 50, step: 1, default: 0},
                            {value: 0}
                        ]
                    },
                    {
                        name: "Lippenstift",
                        method: "setHeadOverlay",
                        settings: [
                            {value: 8},
                            {name: "Art ", price: 100, value: 0, min: 0, max: 9, step: 1, default: 0},
                            {name: "Deckkraft ", price: 100, value: 0, min: 0, max: 1, step: 0.1, default: 0},
                            {name: "Farbe ", price: 100, value: 0, min: 0, max: 50, step: 1, default: 0},
                            {value: 0}
                        ]
                    }
                ]
            }
        },
        methods: {
            changeGender() {
                this.trigger("changeGender", this.gender.male ? 0x9C9EFFD8 : 0x705E61F2)
                this.updateParts()
                this.gender.male = !this.gender.male
            },
            finish() {
                this.triggerServer("UpdateCharacterCustomization", this.convertJsonToServer(), this.calculateTotal)
                this.trigger("exit", true)
				this.dismiss()
			},
			close() {
                this.triggerServer("StopCustomization")
				this.dismiss();
			},
			setActive(event, index) {
				this.currentPartIndex = index;
				document.querySelectorAll(".parts li").forEach((element) => element.classList = "");
				event.target.classList += "active";
			},
			convertJsonFromServer(data) {
                //console.log("fromserver",data);
				this.gender.male = this.gender.default = data.Gender == 0 ? true : false
				for (const index in this.parts) {
					let part = this.parts[index];
					let settings = part.settings
					if (part.method == "setHeadBlendData") {
						this.setValue(settings[0], data.Parents.MotherShape)
						this.setValue(settings[1], data.Parents.FatherShape)
						this.setValue(settings[3], data.Parents.MotherSkin)
						this.setValue(settings[4], data.Parents.FatherSkin)
						this.setValue(settings[6], data.Parents.Similarity)
						this.setValue(settings[7], data.Parents.SkinSimilarity)
					}
					else if (part.method == "setFaceFeature") {
						for (let i = 0; i < 20; i++) {
							this.setValue(settings[i], data.Features[i])
						}
					}
					else if (part.method == "setComponentVariation") {
						this.setValue(settings[1], data.Hair.Hair)
					}
					else if (part.method == "setHairColor") {
						this.setValue(settings[0], data.Hair.Color)
						this.setValue(settings[1], data.Hair.HighlightColor)
					}
					else if (part.method == "setEyeColor") {
						this.setValue(settings[0], data.EyeColor)
					}
					else if (part.method == "setHeadOverlay") {
                        /*console.log("set val", settings[0].value)
                        console.log("ap val", data.Appearance[1].Value)
                        console.log("ap op", data.Appearance[1].Opacity)
                        console.log("beardcol", data.BeardColor)*/
                        if (settings[0].value == 2) {
                            this.setValue(settings[1], data.Appearance[2].Value)
                            this.setValue(settings[2], data.Appearance[2].Opacity)
                            this.setValue(settings[3], data.EyebrowColor)
                        }
                        else if (settings[0].value == 3) {
                            this.setValue(settings[1], data.Appearance[3].Value)
                            this.setValue(settings[2], data.Appearance[3].Opacity)
                        }
                        else if (settings[0].value == 4) {
                            this.setValue(settings[1], data.Appearance[4].Value)
                            this.setValue(settings[2], data.Appearance[4].Opacity)
                        }
                        else if (settings[0].value == 5) {
                            this.setValue(settings[1], data.Appearance[5].Value)
                            this.setValue(settings[2], data.Appearance[5].Opacity)
                            this.setValue(settings[3], data.BlushColor)
                        }
                        else if (settings[0].value == 8) {
                            this.setValue(settings[1], data.Appearance[8].Value)
                            this.setValue(settings[2], data.Appearance[8].Opacity)
                            this.setValue(settings[3], data.LipstickColor)
                        }
					}
				}
			},
			setValue(value, data) {
				value.value = parseFloat(data)
				value.default = parseFloat(data)
			},
			convertJsonToServer() {
				let json = {}
				json.Gender = this.gender.male ? 0 : 1
				json.Parents = {}
				json.Parents.FatherShape = this.parts[0].settings[1].value
				json.Parents.MotherShape = this.parts[0].settings[0].value
				json.Parents.FatherSkin = this.parts[0].settings[4].value
				json.Parents.MotherSkin = this.parts[0].settings[3].value
				json.Parents.Similarity = this.parts[0].settings[6].value
				json.Parents.SkinSimilarity = this.parts[0].settings[7].value
				json.Features = []
				for (let i = 0; i < 20; i++) {
					json.Features[i] = this.parts[1].settings[i].value
				}
				json.Hair = {}
				json.Hair.Hair = this.parts[2].settings[1].value
				json.Hair.Color = this.parts[3].settings[0].value
                json.Hair.HighlightColor = this.parts[3].settings[1].value
                json.Appearance = this.char.Appearance
                json.Appearance[2] = {Value: this.parts[5].settings[1].value, Opacity: this.parts[5].settings[2].value}
                json.Appearance[3] = {Value: this.parts[6].settings[1].value, Opacity: this.parts[6].settings[2].value}
                json.Appearance[4] = {Value: this.parts[7].settings[1].value, Opacity: this.parts[7].settings[2].value}
                json.Appearance[5] = {Value: this.parts[8].settings[1].value, Opacity: this.parts[8].settings[2].value}
                json.Appearance[8] = {Value: this.parts[9].settings[1].value, Opacity: this.parts[9].settings[2].value}
                json.Tattoos = this.char.Tattoos
                json.EyebrowColor = this.parts[5].settings[3].value
                json.BeardColor = this.char.BeardColor
                json.EyeColor = this.parts[4].settings[0].value
                json.BlushColor = this.parts[8].settings[3].value
                json.LipstickColor = this.parts[9].settings[3].value
                json.ChestHairColor = this.char.ChestHairColor
                console.log("output", JSON.stringify(json))
				return JSON.stringify(json)
            },
            rotate(rot) {
                this.trigger("rotate", rot)
            },
            updateParts() {
                for (let i = 0; i < this.parts.length; i++) {
                    this.trigger("changeCharacterPart", JSON.stringify(this.parts[i]))
                }
            }
        },
        watch: {
            parts: {
                handler() {
                    this.trigger("changeCharacterPart", JSON.stringify(this.parts[this.currentPartIndex]))
                },
                deep: true
            }
        },
        computed: {
            calculateTotal() {
				//let extra = this.gender.male != this.gender.default ? this.gender.price : 0
                return this.parts.reduce((total, part) => {
                    return total + part.settings.reduce((total, setting) => {
                        if (setting.value != setting.default &&
                            setting.price != null) {
                            return total + setting.price
						}
                        return total
                    }, 0)
                }, this.gender.male != this.gender.default ? this.gender.price : 0)
            },
            getValidSettings() {
                return this.parts[this.currentPartIndex].settings.filter((setting) => setting.name)
            }
        },
        mounted() {
            console.log("old char", this.char)
            this.convertJsonFromServer(this.char)
        }
    })
</script>

<style scoped>
h5 {
    text-align: center;
    margin-bottom: 2rem;
}
#creator > div {
    width: auto;
    margin: 1rem;
}
.parts li {
    line-height: 2rem;
    border-bottom: 0.1rem solid rgba(0, 0, 0, 0.2);
    background-color: rgba(0, 0, 0, 0.1);
    margin: 0.5rem;
    padding-left: 1rem;
}
.parts li:hover {
    color: #2196F3;
}
.parts li.active {
    color: #2196F3;
    border-bottom: 0.1rem solid #2196F3;
}
input[type=range] {
    -webkit-appearance: none;
    width: 12rem;
    border: none;
    position: relative;
    top: 8px;
}
input[type=range]::-webkit-slider-thumb {
    -webkit-appearance: none;
    height: 1rem;
    width: 2rem;
    border-radius: 4px;
    cursor: pointer;
    margin-top: -1rem;
    background-color: #9E9E9E;
    border-bottom: #000 solid 0.1rem;
}
input[type=range]::-webkit-slider-runnable-track {
    height: 0.5rem;
    cursor: pointer;
    background: #2196F3;
    border-radius: 4px;
}
.rotate {
    position: absolute;
    bottom: 0;
    left: 50%;
}
.rotate > i {
    font-size: 3rem;
    color: #2196F3;
    margin: 1rem;
}
</style>